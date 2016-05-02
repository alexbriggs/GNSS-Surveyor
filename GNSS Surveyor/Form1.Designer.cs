/// <summary>
/// Form1.Designer.cs - GNSS Surveyor solutions main form.
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

namespace GNSS_Surveyor
{
    /// <summary>
    /// Form class which forms the 'coordinator' for this program. The form handlers user inputs, such as selecting and opening com ports, 
    /// interacting with the logging controls, and allows the user to initiate export of the logged data. Onto this form several user controls have
    /// been added. For a more detailed description of each control, see indiviual .cs files.
    /// </summary>
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.latvalue = new System.Windows.Forms.TextBox();
            this.lonvalue = new System.Windows.Forms.TextBox();
            this.altvalue = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.button2 = new System.Windows.Forms.Button();
            this.elementHost1 = new System.Windows.Forms.Integration.ElementHost();
            this.bingMaps1 = new GNSS_Surveyor.BingMaps();
            this.speedo1 = new GNSS_Surveyor.Speedo();
            this.compass1 = new GNSS_Surveyor.Compass();
            this.gnss1 = new GNSS_Surveyor.GNSS();
            this.SuspendLayout();
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(13, 4);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 32);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(143, 50);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(108, 23);
            this.button3.TabIndex = 7;
            this.button3.Text = "Zoom IN";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.MouseClick += new System.Windows.Forms.MouseEventHandler(this.button3_MouseClick);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(258, 50);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(101, 23);
            this.button4.TabIndex = 8;
            this.button4.Text = "Zoom OUT";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(585, -1);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(75, 53);
            this.button5.TabIndex = 9;
            this.button5.Text = "Start Logging";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(377, 4);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(96, 21);
            this.comboBox2.TabIndex = 10;
            this.comboBox2.SelectedValueChanged += new System.EventHandler(this.comboBox2_SelectedValueChanged);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(666, -1);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(75, 53);
            this.button6.TabIndex = 11;
            this.button6.Text = "Finish Logging";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Enabled = false;
            this.button7.Location = new System.Drawing.Point(604, 50);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(123, 23);
            this.button7.TabIndex = 12;
            this.button7.Text = "Finish Feature";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // latvalue
            // 
            this.latvalue.Location = new System.Drawing.Point(480, 4);
            this.latvalue.Name = "latvalue";
            this.latvalue.Size = new System.Drawing.Size(100, 20);
            this.latvalue.TabIndex = 13;
            // 
            // lonvalue
            // 
            this.lonvalue.Location = new System.Drawing.Point(480, 30);
            this.lonvalue.Name = "lonvalue";
            this.lonvalue.Size = new System.Drawing.Size(100, 20);
            this.lonvalue.TabIndex = 14;
            this.lonvalue.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // altvalue
            // 
            this.altvalue.Location = new System.Drawing.Point(480, 53);
            this.altvalue.Name = "altvalue";
            this.altvalue.Size = new System.Drawing.Size(100, 20);
            this.altvalue.TabIndex = 15;
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(798, 362);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(105, 60);
            this.button2.TabIndex = 16;
            this.button2.Text = "Export Data";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // elementHost1
            // 
            this.elementHost1.Location = new System.Drawing.Point(26, 79);
            this.elementHost1.Name = "elementHost1";
            this.elementHost1.Size = new System.Drawing.Size(742, 358);
            this.elementHost1.TabIndex = 5;
            this.elementHost1.Text = "elementHost1";
            this.elementHost1.Child = this.bingMaps1;
            // 
            // speedo1
            // 
            this.speedo1.BackColor = System.Drawing.SystemColors.ButtonShadow;
            this.speedo1.Location = new System.Drawing.Point(774, 206);
            this.speedo1.Name = "speedo1";
            this.speedo1.Size = new System.Drawing.Size(150, 150);
            this.speedo1.TabIndex = 4;
            // 
            // compass1
            // 
            this.compass1.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.compass1.Location = new System.Drawing.Point(774, 30);
            this.compass1.Name = "compass1";
            this.compass1.Size = new System.Drawing.Size(150, 150);
            this.compass1.TabIndex = 3;
            this.compass1.Load += new System.EventHandler(this.compass1_Load);
            // 
            // gnss1
            // 
            this.gnss1.Location = new System.Drawing.Point(156, -1);
            this.gnss1.Name = "gnss1";
            this.gnss1.Size = new System.Drawing.Size(215, 56);
            this.gnss1.TabIndex = 2;
            this.gnss1.updateMap += new System.EventHandler(this.gnss1_updateMap);
            this.gnss1.Fix += new System.EventHandler(this.gnss1_Fix);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.ClientSize = new System.Drawing.Size(936, 449);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.altvalue);
            this.Controls.Add(this.lonvalue);
            this.Controls.Add(this.latvalue);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.elementHost1);
            this.Controls.Add(this.speedo1);
            this.Controls.Add(this.compass1);
            this.Controls.Add(this.gnss1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Button button1;
        private GNSS gnss1;
        private Compass compass1;
        private Speedo speedo1;
        private System.Windows.Forms.Integration.ElementHost elementHost1;
        private BingMaps bingMaps1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.TextBox latvalue;
        private System.Windows.Forms.TextBox lonvalue;
        private System.Windows.Forms.TextBox altvalue;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button button2;
    }
}

