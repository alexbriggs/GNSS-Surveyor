/// <summary>
/// Speedo.cs - User control to display speed of travel. 
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
    /// User control to display the speed of motion as a visual graphic. The user can set the value of the dial, which operates between 0-12mph.
    /// This is hard coded, however further work could allow the user to set the min max of the gauge. The user should technically set the value of
    /// a between 0-1, with 0 representing 0mph and 1 representing 12mph. Again this could be made more extensible with further work.
    /// </summary>
    public partial class Speedo : UserControl
    {
        public Speedo()
        {
            InitializeComponent();
        }
        //component variables
        private double startx, starty, endx, endy, bx, by, rx, ry, lx, ly, length, angleDegrees, angleRadians;
        private double toplength = 0.0;
        private double bottomlength = 0.0;
        Brush b = new SolidBrush(Color.Red);
        Graphics g;
        Graphics g2;
        Bitmap bm;
        private double Value;
        public double a
        {
            set
            {
                Value = value;
                if (Value > 1) { Value = 1; }
                drawneedle(Value,g2);

            }
        }
        /// <summary>
        /// Onload method. Sets up the speedo to the default position.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Speedo_Load(object sender, EventArgs e)
        {
            startx = this.Width / 2;
            starty = this.Height / 2;
            length = (this.Height / 2) * 0.9;

            toplength = this.Height / 2 * 0.85;
            bottomlength = this.Height / 2 * 0.6625;
            g = this.CreateGraphics();
            bm = new Bitmap(this.Width, this.Height);
            g2 = Graphics.FromImage(bm);
            drawneedle(1.0, g2);

        }
        /// <summary>
        /// Method to calculate and produce image of speedo. Invalidate called to update the UI.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="graphics"></param>
        public void drawneedle(double value, Graphics graphics)
        {
            //clear any graphics on the graphic object.
            graphics.Clear(Color.DarkGray);
            Pen whitepen = new Pen(Color.White, 2);
            Pen redpen = new Pen(Color.Red, 2);
            //
            for (int i = 0; i < 68; i++)
            {
                //draw lines between 0-225 degrees and 

                double angle = toRadian(((225 + (i * 4) % 360) - 90));
                double topx = startx + Math.Cos(angle) * toplength;
                double topy = starty + Math.Sin(angle) * toplength;
                double bottomx = startx + Math.Cos(angle) * bottomlength;
                double bottomy = starty + Math.Sin(angle) * bottomlength;
                int x1 = Convert.ToInt32(topx);
                int y1 = Convert.ToInt32(topy);
                int x2 = Convert.ToInt32(bottomx);
                int y2 = Convert.ToInt32(bottomy);
                //draw later lines in red
                if (i > 46)
                {
                    graphics.DrawLine(redpen, x1, y1, x2, y2);
                }
                else { graphics.DrawLine(whitepen, x1, y1, x2, y2); }

            }
            //set up variables to hold label xy's
            int X0 = 0, Y0 = 0, X2 = 0, Y2 = 0, X4 = 0, Y4 = 0, X6 = 0, Y6 = 0, X8 = 0, Y8 = 0, X10 = 0, Y10 = 0, X12 = 0, Y12 = 0;
            //get label x y for each label, passing variables in by reference.
            GetLabelXY(225, ref X0, ref Y0);
            GetLabelXY(270, ref X2, ref Y2);
            GetLabelXY(315, ref X4, ref Y4);
            GetLabelXY(360, ref X6, ref Y6);
            GetLabelXY(45, ref X8, ref Y8);
            GetLabelXY(90, ref X10, ref Y10);
            GetLabelXY(135, ref X12, ref Y12);
            //sey up string formatter to center label on point
            StringFormat formatter = new StringFormat();
            formatter.LineAlignment = StringAlignment.Center;
            formatter.Alignment = StringAlignment.Center;



            Font f = new Font("Courier New", 14, FontStyle.Bold, GraphicsUnit.Pixel);
            //draw labels
            graphics.DrawString("0", f, Brushes.White, X0, Y0, formatter);
            graphics.DrawString("2", f, Brushes.White, X2, Y2, formatter);
            graphics.DrawString("4", f, Brushes.White, X4, Y4, formatter);
            graphics.DrawString("6", f, Brushes.White, X6, Y6, formatter);
            graphics.DrawString("8", f, Brushes.White, X8, Y8, formatter);
            graphics.DrawString("10", f, Brushes.White, X10, Y10, formatter);
            graphics.DrawString("12", f, Brushes.White, X12, Y12, formatter);
            //calculate points to draw needle according to speed
            if (value <= 0.5)
            {
                angleDegrees = (225 + ((value * 270))) % 360;
            }
            else
            {
                double a = (value - 0.5) * 270;
                angleDegrees = 0 + a;
            }
            angleRadians = (Math.PI / 180) * (angleDegrees - 90);
            double angleRadiansR = (Math.PI / 180) * ((90 + angleDegrees) % 360);
            endx = startx + Math.Cos(angleRadians) * (length * 0.7);
            endy = starty + Math.Sin(angleRadians) * (length * 0.7);
            bx = startx + Math.Cos(angleRadiansR) * (length / 5);
            by = starty + Math.Sin(angleRadiansR) * (length / 5);
            double ninetydeg = (Math.PI / 180) * 90;
            rx = startx + Math.Cos(angleRadians + ninetydeg) * (length / 8);
            ry = starty + Math.Sin(angleRadians + ninetydeg) * (length / 8);
            lx = startx + Math.Cos(angleRadians - ninetydeg) * (length / 8);
            ly = starty + Math.Sin(angleRadians - ninetydeg) * (length / 8);
            //generate needle polygon point array
            Point[] needle =
            {         new Point((int)endx, (int)endy),
                new Point((int)lx, (int)ly),
                new Point((int)bx, (int)by),
                new Point((int)rx, (int)ry)
                            };
            graphics.FillPolygon(b, needle);
            //invalidate to fire paint method
            this.Invalidate();

        }

        private void Speedo_Paint(object sender, PaintEventArgs e)
        {
            g.DrawImage(bm,0,0);

        }
        /// <summary>
        /// function to calculate the position of the labels, at a given angle 
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        private void GetLabelXY(double angle, ref int x, ref int y)
        {
            double radangle = (angle - 90 % 360) * Math.PI / 180;
            x = Convert.ToInt32(startx + Math.Cos(radangle) * (length * 0.6));
            y = Convert.ToInt32(starty + Math.Sin(radangle) * (length * 0.6));

        }
        /// <summary>
        /// Method to convert decimal degrees to radians, used by c# Math methods.
        /// </summary>
        /// <param name="val"></param>
        /// <returns>Angle in radians</returns>
        private double toRadian(double val)
        {
            return (Math.PI / 180) * val;
        }
    }
}
