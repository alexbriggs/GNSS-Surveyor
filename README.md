# GNSS-Surveyor
A GNSS surveying application which allows the user to connect to a GNSS over COM port, and log points lines and polygons on a BingMaps WPF control.

This application is designed to provide an open source surveying application, which will work with GNSS at any accuracy, provided the device outputs
the standard NMEA0183 protocol, in particular $GPGGA and $GPRMC messages. The application is aimed to be simple to use, with the user able to log and
visualise points, lines and polygons. 

To get started, select the com port to which the device is connected, and click connect. If no connection is possible, the application will retry to
connect twice before an error message is shown. If no error message appears, but the map does not update and GPS stats remain blank, please check the 
device is outputting correctly using com port terminal programs such as the open source teraterm (https://ttssh2.osdn.jp/index.html.en).

Once connected, the user will see their current position, and a range of GNSS statistics, including speed and bearing visualised through the compass 
and speedo controls. In order to log features, select the desired feature type from the drop down box (point, line, polygon).

If logging a point, click start logging and remain in a stationary position, the position is recorded on clicking finish logging and will be the average
position between clicking start and finish. once finish is clicked, the user is free to log another stationary point or change feature type.
If logging a polygon or polyline, the user uses start and finish logging to log stationary points at the verticies of the desired feature. This works
in an identical manor to point logging as described above. The difference with mulipoint features is the user will use finish feature in order to 
complete the feature.

Exporting data is possible using the Export data button. This will require the user to specify an output directory to which three seperate GeoJSON files 
will written for points, lines and polygons. GeoJSON (http://geojson.org/) is a standard for representing geographical features in the commmon JSON format.
GeoJSON may be used with all of the major web mapping api's such as Google Maps and Leaflet, and may also be imported to desktop GIS packages such as ArcGIS
using import tools built into the software.
