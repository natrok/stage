using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace ConsoleApp1
{
    public partial class UserControl : System.Windows.Forms.UserControl
    {

        /* LinearGradientBrush myBrush = new LinearGradientBrush(,);
         myBrush.GradientStops.Add(new GradientStop(Colors.Yellow, 0.0));
         myBrush.GradientStops.Add(new GradientStop(Colors.Orange, 0.5));
         myBrush.GradientStops.Add(new GradientStop(Colors.Red, 1.0));
     */
        public UserControl()
        {
            InitializeComponent();
            fadeBar.Minimum = 0;
            fadeBar.Maximum = 100;
            speedBar.Minimum = 0;
            speedBar.Maximum = 100;
            dropRateBar.Minimum = 0;
            dropRateBar.Maximum = 1000;
            dropRateBumpBar.Minimum = 0;
            dropRateBumpBar.Maximum = 1000;
            numParticlesBar.Minimum = 0;
            numParticlesBar.Maximum = 1000;

        }

        public TextBox FadeBox{
            get { return fadeBox; }          
        }

        public TrackBar FadeBar {
            get { return fadeBar; }
        }


        public TextBox SpeedBox
        {
            get { return speedBox; }
        }

        public TrackBar SpeedBar
        {
            get { return speedBar; }
        }

        public TextBox DropRateBox
        {
            get { return dropRateBox; }
        }

        public TrackBar DropRateBar
        {
            get { return dropRateBar; }
        }


        public TextBox DropRateBumpBox
        {
            get { return dropRateBumpBox; }
        }

        public TrackBar DropRateBumpBar
        {
            get { return dropRateBumpBar; }
        }


        public TextBox NumParticlesBox
        {
            get { return numParticlesBox; }
        }

        public TrackBar NumParticlesBar
        {
            get { return numParticlesBar; }
        }

        private void dropRateBox_TextChanged(object sender, EventArgs e)
        {

        }

    }
}
