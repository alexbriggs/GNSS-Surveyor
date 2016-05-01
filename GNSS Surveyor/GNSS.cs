/// <summary>
/// GNSS.cs - User control to parse data from GNSS source and store current GPS status.
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
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GNSS_Surveyor
{
    /// <summary>
    /// GNSS user control, parses NMEA0183 sentances and displays GNSS fix information for the user. The user may parse nmea sentances by calling
    /// ParseSentence and passsing in the sentance as a string. Once parsed, an onFix event will fire if a valid position is detected. an onupdatemap
    /// event will fire in theory once every second. This is determined by the hertz variable, which is defaulted to 10. The user may set this variable 
    /// dependant of the gps device being used (e.g. if adevice with a position refresh rate of once per second was known to be used, set hertz to 1)
    /// </summary>
    public partial class GNSS : UserControl
    {
        /// <summary>
        /// Component constructor, initialises the component
        /// </summary>
        public GNSS()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Component onload method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GNSS_Load(object sender, EventArgs e)
        {

        }
        //component variables.
        public event EventHandler updateMap;
        public event EventHandler Fix;
        public double Longitude, Latitude, Altitude, Heading, speed;
        public int FixQuality;
        public int SatNo = 0;
        public string FixDisplay = "";
        public Color c;
        private int i = 0;
        public int hertz = 10;
        /// <summary>
        /// Component onupdatemap event handler. Called once per second - determined by the hertz variable. Defaulted to 10hz but 
        /// can be overidden by user.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnupdateMap(EventArgs e)
        {
            EventHandler handler = updateMap;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        /// <summary>
        /// Component on fix handler. Called every time a new position is received from the GNSS.
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnFix(EventArgs e)
        {
            EventHandler handler = Fix;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        /// <summary>
        /// Method to parse and NMEA0183 sentence. 
        /// </summary>
        /// <param name="sSentence"></param>
        public void ParseSentence(String sSentence)
        {
            try
            {
                //Check sentence is complete and valid, by verifying the presence of *, found at end of line.
                if (sSentence.IndexOf('*') == -1)
                {
                    //return if not found
                    return;
                }
                //NMEA0183 is comma separated, convert sentence to array
                sSentence = sSentence.Substring(0, sSentence.IndexOf('*'));
                String[] aSplit = sSentence.Split(',');
                if (aSplit.Length > 0)
                {
                    switch (aSplit[0])
                    {
                        case "$GPGGA":
                            try
                            {
                                if (aSplit[2] == "")
                                {
                                    return;
                                }
                                //convert lat lng values to decimal degrees, correcting for hemisphere
                                Latitude = Convert.ToDouble(aSplit[2].Substring(0, 2));
                                Latitude += Convert.ToDouble(aSplit[2].Substring(2, aSplit[2].Length - 2)) / 60.0;
                                if (aSplit[3] == "S")
                                {
                                    Latitude *= -1;
                                }
                                if (aSplit[4].IndexOf('.') == 2)
                                {
                                    aSplit[4] = "000" + aSplit[4];
                                }
                                Longitude = Convert.ToDouble(aSplit[4].Substring(0, 3));
                                Longitude += Convert.ToDouble(aSplit[4].Substring(3, aSplit[4].Length - 3)) / 60.0;
                                if (aSplit[5] == "W")
                                {
                                    Longitude *= -1;
                                }


                                //parse altitude
                                if (aSplit[11] != "")
                                {
                                    Altitude = Convert.ToDouble(aSplit[11]);

                                }
                                else
                                {
                                    Altitude = 0;

                                }
                                //parse number of locked sats
                                SatNo = Convert.ToInt32(aSplit[7]);
                                //parse fix quality
                                FixQuality = Convert.ToInt32(aSplit[6]);
                                if (FixQuality == 0)
                                {
                                    FixDisplay = "Invalid";
                                    c = Color.Gray;
                                }
                                else if (FixQuality == 1)
                                {
                                    FixDisplay = "Standalone";
                                    c = Color.Red;
                                }
                                else if (FixQuality == 2)
                                {
                                    FixDisplay = "DGPS";
                                    c = Color.Red;

                                }
                                else if (FixQuality == 3)
                                {
                                    FixDisplay = "PPS";
                                    c = Color.Red;
                                }
                                else if (FixQuality == 4)
                                {
                                    FixDisplay = "RTK FIXED";
                                    c = Color.Green;
                                }
                                else if (FixQuality == 5)
                                {
                                    FixDisplay = "Float";
                                    c = Color.Orange;
                                }
                                else if (FixQuality == 6)
                                {
                                    FixDisplay = "Estimated";
                                }
                                else if (FixQuality == 7)
                                {
                                    FixDisplay = "Manual";
                                    c = Color.Red;
                                }
                                else if (FixQuality == 8)
                                {
                                    FixDisplay = "Simulation";
                                    c = Color.Red;
                                }
                                //every 1 second 
                                if (++i == hertz)
                                {
                                    OnupdateMap(null);
                                    i = 0;

                                }
                                OnFix(null);
                            }


                            catch (FormatException e)
                            {

                            }
                            break;
                        case "$GPRMC":
                            //parse heading and speed
                            speed = Convert.ToDouble("0000" + aSplit[7]) * 1.852;
                            Heading = Convert.ToDouble(aSplit[8]);
                            break;
                    }
                    //set component UI with GNSS values.
                    textBox3.BackColor = c;
                    textBox1.Text = Convert.ToString(Latitude);
                    textBox2.Text = Convert.ToString(Longitude);
                    textBox3.Text = Convert.ToString(SatNo);
                    textBox4.Text = Convert.ToString(Altitude);
                }



            }
            catch (Exception e)
            {
            }

        }
    }
}
