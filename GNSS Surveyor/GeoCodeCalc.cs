
/*
 * Written by Chris Pietschmann (http://pietschsoft.com)
 * This code is derived from the code posted in the following location:
 * http://pietschsoft.com/post/2010/03/02/Bing-Maps-JS-Calculate-Area-of-Circle-and-Draw-Circle-on-Map.aspx
 *Code can be found at http://pietschsoft.com/post/2010/06/28/Silverlight-Bing-Maps-Draw-Circle-Around-Latitude-Longitude-Location
*/

using Microsoft.Maps.MapControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maps.MapControl.WPF;
namespace GNSS_Surveyor
{
    public enum DistanceMeasure
    {
        Miles,
        Kilometers
    }
    class GeoCodeCalc
    {
        public const double EarthRadiusInMiles = 3956.0;
        public const double EarthRadiusInKilometers = 6367.0;

        public static double ToRadian(double degrees)
        {
            return degrees * (Math.PI / 180);
        }

        public static double ToDegrees(double radians)
        {
            return radians * (180 / Math.PI);
        }

        public static LocationCollection CreateCircle(Location center, double radius, DistanceMeasure units)
        {
            var earthRadius = (units == DistanceMeasure.Miles ? GeoCodeCalc.EarthRadiusInMiles : GeoCodeCalc.EarthRadiusInKilometers);
            var lat = ToRadian(center.Latitude); //radians
            var lng = ToRadian(center.Longitude); //radians
            var d = radius / earthRadius; // d = angular distance covered on earth's surface
            var locations = new LocationCollection();

            for (var x = 0; x <= 360; x++)
            {
                var brng = ToRadian(x);
                var latRadians = Math.Asin(Math.Sin(lat) * Math.Cos(d) + Math.Cos(lat) * Math.Sin(d) * Math.Cos(brng));
                var lngRadians = lng + Math.Atan2(Math.Sin(brng) * Math.Sin(d) * Math.Cos(lat), Math.Cos(d) - Math.Sin(lat) * Math.Sin(latRadians));

                locations.Add(new Location(ToDegrees(latRadians), ToDegrees(lngRadians)));
            }

            return locations;
        }
    }
}
