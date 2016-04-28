using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GNSS_Surveyor
{
    /// <summary>
    /// Compass control. This control visualises the current bearing the user is travelling along. The update the control simply set the variable a 
    /// with a bearing in degrees. The control utilises double buffering for performance. The compass rotates with an 'up' arrow indicating the direction
    /// of travel.
    /// </summary>
    public partial class Compass : UserControl
    {
        //control variables, including 2 graphics objects and a bitmap to enable double buffering
        Graphics g;
        Graphics g2;
        private double angledeg=0.0;
        Bitmap bm;
        private int startx, starty;
        private double toplength = 0.0;
        private double bottomlength = 0.0;
        private double angle = 0.0;
        private double recipangle = 0.0;
        private double leftangle = 0.0;
        private double rightangle = 0.0;
        public Compass()
        {
            InitializeComponent();
        }
        /// <summary>
        /// Component onload method. Draws the compass in the 'default' postiion. Awaits user to update with a bearing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Compass_Load(object sender, EventArgs e)
        {
            //calculate center xy and distance from center to top and bottom of compass 'ring'
            startx = this.Width / 2;
            starty = this.Height / 2;
            toplength = this.Height / 2 * 0.85;
            bottomlength = this.Height / 2 * 0.6625;
            //define components graphics object.
            g = this.CreateGraphics();
            //create second graphics object from a bitmap which is the same size as the control
            bm = new Bitmap(this.Width, this.Height);
            g2 = Graphics.FromImage(bm);
            //initiate drawing of compass.
            Arrow(angledeg, g2);
        }
        /// <summary>
        /// Method for user to update bearing of compass.
        /// </summary>
        public double a
        {
            set
            {
                angledeg = value;
                if (angledeg >= 0 && angledeg <= 365)
                {
                    angledeg = value;
                    
                }
                else { angledeg = 0; }
                //initiate drawing.
                Arrow(angledeg, g2);
                this.Invalidate();
            }
        }
        /// <summary>
        /// Components paint method, used when new bearing received and control is invalidated.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Compass_Paint(object sender, PaintEventArgs e)
        {
            //swap the current UI with a bitmap from memory.
            g.DrawImage(bm,0,0);
            
        }
        /// <summary>
        /// Function to calculate graphics to be drawn, draws to a bitmap in memory, then invalidates to update UI.
        /// </summary>
        /// <param name="angledeg"></param>
        /// <param name="graphics"></param>
        private void Arrow(double angledeg, Graphics graphics)
        {
            //clear anythign already drawn on graphics object
            graphics.Clear(Color.DarkGray);
            //calculate coordinates for direction arrow.
            int p1y = Convert.ToInt32(1 + (this.Width) / 15);
            int p1x = Convert.ToInt32((this.Width / 2) - ((this.Width / 15) / 2));
            int p2y = Convert.ToInt32(1 + (this.Width) / 15);
            int p2x = Convert.ToInt32((this.Width / 2) + ((this.Width / 15) / 2));
            Point[] arrow = {
                                 new Point(this.Width/2,1),
                                 new Point(p1x,p1y),
                                 new Point(p2x,p2y)
                             };
            //draw arrow.
            graphics.FillPolygon(Brushes.Red, arrow);
            //set up pens for drawing compass.
            Pen whitepen = new Pen(Color.White, 2);
            Pen redpen = new Pen(Color.Red, 2);
            //draw line below arrowhead
            Point t = new Point(this.Width / 2, Convert.ToInt32(this.Height / 2 - toplength));
            Point b = new Point(this.Width / 2, Convert.ToInt32((this.Height / 2) * 0.4125));
            graphics.DrawLine(redpen, t, b);

            double northangle = (360 - angledeg) % 360;
            //create 90 lines which make up the ring 
            for (int i = 0; i < 72; i++)
            {
                //calculate start and end points of the line. calculate by drawing from the center point with 5 degree spacing, ofset to keep 'up' as the direction of travel.
                leftangle = toRadian(((northangle + (i * 5) - 180) % 360));
                rightangle = toRadian(((northangle + (i * 5)) % 360));

                angle = toRadian(((northangle + (i * 5) % 360) - 90));
                recipangle = toRadian(((northangle + (i * 5) + 90) % 360));
                double topx = startx + Math.Cos(angle) * toplength;
                double topy = starty + Math.Sin(angle) * toplength;
                double bottomx = startx + Math.Cos(angle) * bottomlength;
                double bottomy = starty + Math.Sin(angle) * bottomlength;
                int x1 = Convert.ToInt32(topx);
                int y1 = Convert.ToInt32(topy);
                int x2 = Convert.ToInt32(bottomx);
                int y2 = Convert.ToInt32(bottomy);
                //draw red lines at N, E, S, W
                if (i == 0 || i == 18 || i == 36 || i == 54)
                {
                    graphics.DrawLine(redpen, x1, y1, x2, y2);
                }
                else {
                    //draw white lines
                    graphics.DrawLine(whitepen, x1, y1, x2, y2);
                }

            }

            //calculate control angle from each direction. C# angles start at 90 degree and increment anti-clockwise (compared to standard compass)
            leftangle = toRadian(((northangle - 180) % 360));//aka if going north, angle to west in c# would be 180 degrees.
            rightangle = toRadian(((northangle) % 360));//if going east, c# angle would be 0 degrees.

            angle = toRadian(((northangle % 360) - 90));
            recipangle = toRadian(((northangle + 90) % 360));
            //calculate coordinates for lines through center North to south and east to west.
            int crosslength = Convert.ToInt32((this.Width / 2) * 0.34);
            int letterlength = Convert.ToInt32((this.Width / 2) * 0.65);
            int NStopx = Convert.ToInt32(startx + Math.Cos(angle) * crosslength);
            int NStopy = Convert.ToInt32(starty + Math.Sin(angle) * crosslength);
            int NSbottomx = Convert.ToInt32(startx + Math.Cos(recipangle) * crosslength);
            int NSbottomy = Convert.ToInt32(starty + Math.Sin(recipangle) * crosslength);
            int WEx = Convert.ToInt32(startx + Math.Cos(leftangle) * crosslength);
            int WEy = Convert.ToInt32(starty + Math.Sin(leftangle) * crosslength);
            int EWx = Convert.ToInt32(startx + Math.Cos(rightangle) * crosslength);
            int EWy = Convert.ToInt32(starty + Math.Sin(rightangle) * crosslength);
            graphics.DrawLine(whitepen, NStopx, NStopy, NSbottomx, NSbottomy);
            graphics.DrawLine(whitepen, WEx, WEy, EWx, EWy);

            //calculate coordates of N, S, E, W labels
            int Nx = Convert.ToInt32(startx + Math.Cos(angle) * letterlength);
            int Ny = Convert.ToInt32(starty + Math.Sin(angle) * letterlength);
            int Sx = Convert.ToInt32(startx + Math.Cos(recipangle) * letterlength);
            int Sy = Convert.ToInt32(starty + Math.Sin(recipangle) * letterlength);
            int Wx = Convert.ToInt32(startx + Math.Cos(leftangle) * letterlength);
            int Wy = Convert.ToInt32(starty + Math.Sin(leftangle) * letterlength);
            int Ex = Convert.ToInt32(startx + Math.Cos(rightangle) * letterlength);
            int Ey = Convert.ToInt32(starty + Math.Sin(rightangle) * letterlength);
            //use string formatter to center letter over point
            StringFormat formatter = new StringFormat();
            formatter.LineAlignment = StringAlignment.Center;
            formatter.Alignment = StringAlignment.Center;
            Font f = new Font("Courier New", 18, FontStyle.Bold, GraphicsUnit.Pixel);
            //draw labels
            graphics.DrawString("N", f, Brushes.Black, Nx, Ny, formatter);
            graphics.DrawString("S", f, Brushes.Black, Sx, Sy, formatter);
            graphics.DrawString("E", f, Brushes.Black, Ex, Ey, formatter);
            graphics.DrawString("W", f, Brushes.Black, Wx, Wy, formatter);
            //invalidate to swap g2's bitmap with g's UI panel
            this.Invalidate();

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
