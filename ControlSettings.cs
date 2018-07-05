using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class ControlSettings
    {

        private UserControl myUserControl;
        private Particle myParticle;

        private float fadeMin = 0.96f, fadeMax = 0.999f;
        private float dropRateMin = 0.0f, dropRateMax = 0.1f;
        private float speedMin = 0.05f, speedMax = 1.0f;
        private float dropRateBumpMin = 0, dropRateBumpMax = 0.2f;
        //private int numParticlesMin = 1024, numParticlesMax = 58000;
        private float regSpeedMin = 0.01f, regSpeedMax = 1.0f;

        private List<Tuple<double, uint>> froidRampColors, defaultRampColors, hotRampColors, gris;

        public ControlSettings(UserControl myUserControl, Particle myParticle) {

            this.myUserControl = myUserControl;
            this.myParticle = myParticle;

        }
        public void fadeBar_Scroll(object sender, EventArgs e)
        {

            myParticle.FadeOpacity = fadeMin +
                 (float)(myUserControl.FadeBar.Value * (fadeMax - fadeMin)) / myUserControl.FadeBar.Maximum;

            myUserControl.FadeBox.Text = myParticle.FadeOpacity.ToString();
        }

        public void speedBar_Scroll(object sender, EventArgs e)
        {
            float value = (float)(speedMax * myUserControl.SpeedBar.Value) / myUserControl.SpeedBar.Maximum;
            myParticle.SpeedFactor = value;
            myUserControl.SpeedBox.Text = myParticle.SpeedFactor.ToString();
        }

        public void speedRegBar_Scroll(object sender, EventArgs e)
        {
            float value = (float)(regSpeedMax * myUserControl.RegSpeedBar.Value) / myUserControl.RegSpeedBar.Maximum;
            myParticle.SpeedReg = value;
            myUserControl.RegSpeedBox.Text = myParticle.SpeedReg.ToString();
        }
        public void dropRate_Scroll(object sender, EventArgs e)
        {
            float dropRate = (float)(dropRateMax * myUserControl.DropRateBar.Value) / myUserControl.DropRateBar.Maximum;
            myParticle.DropRate = dropRate;
            myUserControl.DropRateBox.Text = myParticle.DropRate.ToString();
        }

        public void dropRateBump_Scroll(object sender, EventArgs e)
        {
            float value = (float)(dropRateBumpMax * myUserControl.DropRateBumpBar.Value) / myUserControl.DropRateBumpBar.Maximum;
            myParticle.DropRateBump = value;
            myUserControl.DropRateBumpBox.Text = myParticle.DropRateBump.ToString();
        }

        public void checkBoxPoint_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        public void checkBoxWave_CheckedChanged(object sender, EventArgs e)
        {
            myParticle.IsWave = myUserControl.CheckBoxWave.Checked; 
        }

        public void comboBoxColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            froidRampColors = new List<Tuple<double, uint>>()
            {
            new Tuple<double, uint>(0.0, 0xffeff3ff),
            new Tuple<double, uint>(0.1, 0xffdeebf7),
            new Tuple<double, uint>(0.2, 0xffc6dbef),
            new Tuple<double, uint>(0.3, 0xff9ecae1),
            new Tuple<double, uint>(0.4, 0xff6baed6),
            new Tuple<double, uint>(0.5, 0xff4292c6),
            new Tuple<double, uint>(0.6, 0xff2171b5),
            new Tuple<double, uint>(1.0, 0xff084594)
            };

            defaultRampColors = new List<Tuple<double, uint>>()
            {
            new Tuple<double, uint>(0.0, 0xff3288bd),
            new Tuple<double, uint>(0.1, 0xff66c2a5),
            new Tuple<double, uint>(0.2, 0xffabdda4),
            new Tuple<double, uint>(0.3, 0xffe6f598),
            new Tuple<double, uint>(0.4, 0xfffee08b),
            new Tuple<double, uint>(0.5, 0xfffdae61),
            new Tuple<double, uint>(0.6, 0xfff46d43),
            new Tuple<double, uint>(1.0, 0xffd53e4f)
            };

            hotRampColors = new List<Tuple<double, uint>>()
            {
            new Tuple<double, uint>(0.0, 0xffffffd9),
            new Tuple<double, uint>(0.1, 0xffedf8b1),
            new Tuple<double, uint>(0.2, 0xffc7e9b4),
            new Tuple<double, uint>(0.3, 0xff7fcdbb),
            new Tuple<double, uint>(0.4, 0xff41b6c4),
            new Tuple<double, uint>(0.5, 0xff1d91c0),
            new Tuple<double, uint>(0.6, 0xff225ea8),
            new Tuple<double, uint>(1.0, 0xff0c2c84)
            };

            gris = new List<Tuple<double, uint>>()
            {
            new Tuple<double, uint>(0.0, 0xff7f7f7f),
            new Tuple<double, uint>(0.1, 0xff7f7f7f),
            new Tuple<double, uint>(0.2, 0xff7f7f7f),
            new Tuple<double, uint>(0.3, 0xff7f7f7f),
            new Tuple<double, uint>(0.4, 0xff7f7f7f),
            new Tuple<double, uint>(0.5, 0xff7f7f7f),
            new Tuple<double, uint>(0.6, 0xff7f7f7f),
            new Tuple<double, uint>(1.0, 0xff7f7f7f)
            };

            //myParticle.setColorRamp(froidRampColors);

            int color = myUserControl.ComboBoxColor.SelectedIndex;
            switch (color)
            {
                case 0:
                    myParticle.setColorRamp(defaultRampColors);
                    break;
                case 1:
                    myParticle.setColorRamp(froidRampColors);
                    break;
                case 2:
                    myParticle.setColorRamp(hotRampColors);
                    break;
                case 3:
                    myParticle.setColorRamp(gris);
                    break;
            }


        }

    }
}
