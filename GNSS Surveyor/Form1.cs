using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.IO.Ports;
using System.Reflection;

namespace GNSS_Surveyor
{
    /// <summary>
    /// The programs main form, used to display user controls and co-ordinate the application.
    /// </summary>
    public partial class Form1 : Form
    {
        /// <summary>
        /// This class handles the GNSS over serial port. 
        /// </summary>
        public class GNSSSensor
        {
            //class variables
            bool bRunning = true;
            public string port;
            public Form1 form;
            public Thread thread;
            SerialPort sp;
            /// <summary>
            /// Constructor method, instatiates a thread on which to run the Sense method.
            /// </summary>
            /// <param name="port"></param>
            /// <param name="form"></param>
            public GNSSSensor(string port, Form1 form)
            {
                this.port = port;
                this.form = form;
                thread = new Thread(this.Sense);
                thread.Start();
            }

            /// <summary>
            /// Method to handle stopping the serial port and closing the application without leaving open threads. 
            /// </summary>
            public void Close()
            {
                bRunning = false;
                if (sp!=null){
                    thread.Abort();
                    sp.Close();
                    sp = null;
                }
                
            }
            /// <summary>
            /// Method to open the sensor port, and delegate the input stream once the thread is open.
            /// </summary>
            public void Sense()
            {
                sp = new SerialPort(port);
                bool opened = false;
                int attempts = 0;
                //attempt to open the port.
                while (!opened)
                {
                    if (attempts > 2)
                    {
                        //if failed after 2 attempts, inform user and clean up the object.
                        MessageBox.Show("unable to connect to " + port);
                        this.Close();
                    }
                    try
                    {
                        //set basic serial port options and attempt to open.
                        sp.BaudRate = 38400;
                        sp.ReadTimeout = SerialPort.InfiniteTimeout;
                        sp.Open();
                        opened = true;
                    }
                    catch (Exception e)
                    {
                        //retry
                        opened = false;
                        attempts++;
                    }
                }
                while (bRunning)
                {
                    //when open continuously read from the port, invoking the method on the main form, passing the stream readline to the gnss controls parse sentance.
                        String sLine = sp.ReadLine();
                        MethodInvoker mi = delegate { form.gnss1.ParseSentence(sLine); };
                        form.BeginInvoke(mi);
                }
            }
        }

        /// <summary>
        /// Form 1 constructor, initializes the GUI.
        /// </summary>
        public Form1()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Form onload handler, fires once the form has initialized.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            //iterate through the systems connected serial ports, add to a list for the user to choose from.
            string[] ports = SerialPort.GetPortNames();
            for (int i = 0; i < ports.Length; i++)
            {
                comboBox1.Items.Add(ports[i]);
            }
            comboBox1.SelectedIndex = 0;
            //add different logging modes to drop down list for user to choose from.
            comboBox2.Items.Add("Point");
            comboBox2.Items.Add("Line");
            comboBox2.Items.Add("Polygon");
            comboBox2.SelectedIndex = 0;
            
        }

        GNSSSensor s;
        /// <summary>
        /// Handler for the 'connect' button. 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            //use the user selected port name, and instantiate a GNSSSensor accordingly.
            string port = comboBox1.SelectedItem.ToString();
            s = new GNSSSensor(port,this);
        }
        /// <summary>
        /// Override method for the gnss user controls 'updatemap' method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gnss1_updateMap(object sender, EventArgs e)
        {
            //update the compass and speedo with the latest values, and update map position.
            compass1.a = gnss1.Heading;
            speedo1.a = gnss1.speed / 12;
            bingMaps1.setPosition(gnss1.Latitude, gnss1.Longitude);
        }

        private void button2_Click(object sender, EventArgs e)
        {


        }
        /// <summary>
        /// Button to Zoom in on map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_MouseClick(object sender, MouseEventArgs e)
        {
            bingMaps1.zoomin();
        }
        /// <summary>
        /// Button to zoom out of map
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            bingMaps1.zoomout();
        }

       
        public bool logging = false;
        public int loggedpoints = 0;
        public double averageLat = 0;
        public double averageLon = 0;
        public double averageHeight = 0;
        /// <summary>
        /// gnss usercontrol fix override method.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gnss1_Fix(object sender, EventArgs e)
        {
            //if user is logging, add coordinate to 
            if (logging)
            {
                averageLat = averageLat + gnss1.Latitude;
                averageLon = averageLon + gnss1.Longitude;
                averageHeight = averageHeight + gnss1.Altitude;
                loggedpoints++;
            }
        }

        bool multipoint = false;
        /// <summary>
        /// Start Button click handler.
        /// Method to calculate the position of the user, averaging the GNSS coordinates over the duration of the logging period.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            loggedpoints = 0;
            averageLat = 0;
            averageLon = 0;
            averageHeight = 0;
            logging = true;
            //if Point selected, only one 'vertex' in feature, no need for finish feture button.
            if (comboBox2.SelectedItem.ToString() != "Point")
            {
                multipoint = true;
                button7.Enabled = false;
            }
            comboBox2.Enabled = false;
            timer1.Enabled = true;
        }
        /// <summary>
        /// Finish button click handler.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            //if not a multi-vertex feature
            if (!multipoint)
            {
                //use the bingMaps user control addPoint method, stop logging and clean up.
                bingMaps1.addPoint(averageLat / loggedpoints, averageLon / loggedpoints, averageHeight / loggedpoints);
                logging = false;
                comboBox2.Enabled = true;
                timer1.Enabled = false;
                button2.Enabled = true;
            }
            else
            {
                //if multiple features, call the addVertex method, and reset for next vertex
                bingMaps1.addVertex(averageLat / loggedpoints, averageLon / loggedpoints, comboBox2.SelectedItem.ToString());
                logging = false;
                button7.Enabled = true;
                timer1.Enabled = false;
                button2.Enabled = true;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Timer tick handler.
        /// Updates the logged Position 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            latvalue.Text = (averageLat / loggedpoints).ToString();
            lonvalue.Text = (averageLon / loggedpoints).ToString();
            altvalue.Text = (averageHeight / loggedpoints).ToString();
        }
        /// <summary>
        /// Finish feature click handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            //call the bingMaps user controls finishFeature method, and clean up to log next feature
            bingMaps1.finishFeature();
            comboBox2.Enabled = true;
            button7.Enabled = false;
           
        }
        /// <summary>
        /// Export button handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click_1(object sender, EventArgs e)
        {
            //open file dialogue and request output directory from user
            FolderBrowserDialog f = new FolderBrowserDialog();
            DialogResult dr = f.ShowDialog();
            //if valid directory selected
            if(dr == DialogResult.OK)
            {
                //get geojson string from bingMaps control and write to output directory. Repeat for each feature type, separate output files.
                string polys = bingMaps1.ExportPolys();
                polys = polys.Replace("'", "\"");
                StreamWriter sw = new StreamWriter(f.SelectedPath + "/Polys.geojson");
                sw.Write(polys);
                sw.Flush();
                sw.Close();
                string points = bingMaps1.ExportPoints();
                points = points.Replace("'", "\"");
                StreamWriter sw2 = new StreamWriter(f.SelectedPath+"/Points.geojson");
                sw2.Write(points);
                sw2.Flush();
                sw2.Close();
                string lines = bingMaps1.ExportLines();
                lines = lines.Replace("'", "\"");
                StreamWriter sw3 = new StreamWriter(f.SelectedPath+"/Lines.geojson");
                sw3.Write(lines);
                sw3.Flush();
                sw3.Close();
                MessageBox.Show("Export Complete!");
            }
            else
            {
                //ask for a valid directory
                MessageBox.Show("Please select a valid directory!");
            }
        }
        /// <summary>
        /// On Form closing handler, cleans up the GNSSSensor object, closing all threads
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (s != null)
            {
                //call the GNSSSensor's Close method.
                s.Close();
            }
        }
        private void compass1_Load(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {

        }
    }
}
