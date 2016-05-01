/// <summary>
/// BingMaps.xaml.cs - Mapping user control implementing Bing Maps WPF api. Stores and exports geographical data.
//Copyright(C) 2016  Alexander Briggs

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.If not, see<http://www.gnu.org/licenses/>.
/// </summary>


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Maps.MapControl.WPF;
using System.Reflection;

using System.IO;

namespace GNSS_Surveyor
{
    /// <summary>
    /// Interaction logic for BingMaps.xaml. This WPF control hosts a bingmaps map control. This serves to visualise data logged by the user,
    /// and effectively store the data until saved by the user. When required, the control may export the logged features as geoJSON. This control 
    /// will work online or off, however users will see an error message whilst offline as the control is unavle to verify the credentials supplied.
    /// </summary>
    public partial class BingMaps : UserControl
    {
        public BingMaps()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Add point method. Used to add a single point to the map.
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <param name="alt"></param>
        public void addPoint(double lat, double lon, double alt)
        {
            //Point must be represented as circle 'polyon'
            //initiate polygon object
            MapPolygon point = new MapPolygon();
            //convert lat lon doubles to Map control Location object.
            Microsoft.Maps.MapControl.WPF.Location pos = new Microsoft.Maps.MapControl.WPF.Location(lat, lon);
            //set polygon style
            point.Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue);
            point.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
            point.StrokeThickness = 1;
            point.Name = "point";
            //use DataContext object to store point coordinates.
            point.DataContext = "[" + lon + "," + lat + "]";
            //calculate circle verticies, radius of circle is 2.5m. Credit Chris Pietschmann see GeoCodeCalc.cs
            var locations = GeoCodeCalc.CreateCircle(pos, 0.0025, DistanceMeasure.Kilometers);
            point.Locations = locations;
            //add circle to map
            myMap.Children.Add(point);
        }
        //variables to hold poly and line objects
        MapPolygon poly;
        MapPolyline line;
        bool editing = false;
        /// <summary>
        /// Add vertex Method. Used to add multipoint polygons or lines (determined by featureType parameter) to the map.
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        /// <param name="featureType"></param>
        public void addVertex(double lat, double lon, string featureType)
        {
            //parse lat lon to mapcontrol Location object
            Microsoft.Maps.MapControl.WPF.Location pos = new Microsoft.Maps.MapControl.WPF.Location(lat, lon);
            //Switch on the featureType
            switch (featureType)
            {
                //if polygon
                case "Polygon":
                   //if not editing - first vertex added - initialize polygon object
                    if (!editing)
                    {
                        poly = new MapPolygon();
                        poly.Fill = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue);
                        poly.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
                        poly.StrokeThickness = 1;
                        poly.Locations = new LocationCollection();
                        //add polygon to map.
                        myMap.Children.Add(poly);
                        editing = true;
                    }
                    //add position to Locations array
                    poly.Locations.Add(pos);
                    break;
                    //if Line
                case "Line":
                    //if not editing - first vertex added - initialize polyline object
                    if (!editing)
                    {
                        //instatiate line and style
                        line = new MapPolyline();
                        line.Stroke = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Black);
                        line.StrokeThickness = 1;
                        line.Locations = new LocationCollection();
                        //add line to map
                        myMap.Children.Add(line);
                        editing = true;
                    }
                    //push new location to the lines Locations array
                    line.Locations.Add(pos);
                    break;
            }
        }
        /// <summary>
        /// Method called when a user has finished logging a multipoint feature
        /// </summary>
        public void finishFeature()
        {
            //set editing to false. new polygon/line object will be instantiated the next time the user logs a poly/line
            editing = false;
        }
        //first fix boolean varialbe
        Boolean firstfix = true;
        /// <summary>
        /// Method to set current map view to defined lat lon
        /// </summary>
        /// <param name="lat"></param>
        /// <param name="lon"></param>
        public void setPosition(double lat, double lon)
        {
            //parse lat lon to mapcontrol Location object
            Microsoft.Maps.MapControl.WPF.Location gpsPos = new Microsoft.Maps.MapControl.WPF.Location(lat, lon);
            //center map on position
            myMap.Center = gpsPos;
            //if first fix set zoom level - disable this for subsequent fixes.
            if (firstfix)
            {
                firstfix = false;
                myMap.ZoomLevel = 18;
            }
            //calculate locations of a circle of radius 5m around the current poitision
            var locations = GeoCodeCalc.CreateCircle(gpsPos, 0.005, DistanceMeasure.Kilometers);
            //set gps polygon (defined in XAML) Locations to the circle calculated above.
            gps.Locations = locations;
        }
        /// <summary>
        /// Method to Zoom in on map
        /// </summary>
        public void zoomin()
        {
            //max zoom =19, if less than, increase zoom level
            if (myMap.ZoomLevel < 19)
            {
                myMap.ZoomLevel++;
            }
        }
        /// <summary>
        /// Method to Zoom out
        /// </summary>
        public void zoomout()
        {
            //min zoom =1, if more than, decrease zoom level
            if (myMap.ZoomLevel > 1)
            {
                myMap.ZoomLevel--; ;
            }
        }
        /// <summary>
        /// Method to export all polygons on the map as GeoJSON
        /// </summary>
        /// <returns>Polygon GeoJSON as a string</returns>
        public string ExportPolys()
        {
            //write first line of GeoJSON
            string geojson = "{ 'type': 'FeatureCollection', 'features': [";
            //get all polygon on the map
            var polygons = this.myMap.Children.OfType<MapPolygon>();
            //conert to list of polygons
            List<MapPolygon> polys = polygons.ToList();
            //iterate through list
            foreach (MapPolygon poly in polys)
            {
                //if polygon not a 'point' polygon and not the GPS marker polygon
                if (poly.Name != "gps" && poly.Name != "point")
                {
                    //write Polygon GeoJSON
                    geojson += @"{ 'type': 'Feature',
         'geometry': {
                    'type': 'Polygon',
           'coordinates': [
             [";
                    //iterate through locations and add to geojson
                    for (int i = 0; i < poly.Locations.Count(); i++)
                    {
                        geojson += "[" + poly.Locations[i].Longitude + "," + poly.Locations[i].Latitude + "],";
                    }
                    //add first point as last point and a properties object to validate geojson 
                    geojson += "[" + poly.Locations[0].Longitude + "," + poly.Locations[0].Latitude + "]]]},'properties':{}},";
                }

            }
            geojson = geojson.Substring(0, geojson.Length - 1);
            geojson += "]}";
            //close geojson object and return
            return geojson;
        }
        /// <summary>
        /// Method to epxport points logged on the map control.
        /// </summary>
        /// <returns> Point GeoJSON as a string</returns>
        public string ExportPoints()
        {
            string geojson = "{ 'type': 'FeatureCollection', 'features': [";
            var polygons = this.myMap.Children.OfType<MapPolygon>();
            List<MapPolygon> polys = polygons.ToList();
            foreach (MapPolygon poly in polys)
            {
                //iterate through polygons, if Name = point, write GeoJSON
                if (poly.Name == "point")
                {
                    geojson += @"{ 'type': 'Feature',
         'geometry': {
                    'type': 'Point',
           'coordinates':";
                    //DataContext holds the coordinates
                    geojson += poly.DataContext;
                    geojson += "},'properties':{}},";
                }

            }
            geojson = geojson.Substring(0, geojson.Length - 1);
            geojson += "]}";

            return geojson;
        }
        /// <summary>
        /// Method for exporting line objects as GeoJSON
        /// </summary>
        /// <returns>Line GeoJSON as a string</returns>
        public string ExportLines()
        {
            string geojson = "{ 'type': 'FeatureCollection', 'features': [";
            var polylines = this.myMap.Children.OfType<MapPolyline>();
            List<MapPolyline> lines = polylines.ToList();
            foreach (MapPolyline l in lines)
            {
                //iterate trhough polylines, write geoJSON for each
                geojson += @"{ 'type': 'Feature',
         'geometry': {
                    'type': 'LineString',
           'coordinates': [
             ";
                for (int i = 0; i < l.Locations.Count(); i++)
                {
                    geojson += "[" + l.Locations[i].Longitude + "," + l.Locations[i].Latitude + "],";
                }
                geojson += "[" + l.Locations[0].Longitude + "," + l.Locations[0].Latitude + "]]},'properties':{}},";
            }


            geojson = geojson.Substring(0, geojson.Length - 1);
            geojson += "]}";

            return geojson;
        }

    }
}
