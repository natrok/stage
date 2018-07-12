using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class ControlSettings
    {

        private UserView UV;
        private Particle myParticle;




        private float fadeMin = 0.96f, fadeMax = 0.999f;
        private float speedMin = 0.05f, speedMax = 1.0f;
        private float opacityMin = 0.1f, opacityMax = 1.0f;
        private int particleMin = 1000, particleMax = 100000;

        private bool isChangedParticle = false;

        private List<Tuple<double, uint>> froidRampColors, defaultRampColors, hotRampColors, gris;

        public ControlSettings(UserView UV, Particle myParticle) {

            this.UV = UV;
            this.myParticle = myParticle;

        }
        public void fadeBar_Scroll(object sender, EventArgs e)
        {

            myParticle.Fade = fadeMin +
                 (float)(UV.FadeBar.Value * (fadeMax - fadeMin)) / UV.FadeBar.Maximum;

            UV.FadeBox.Text = myParticle.Fade.ToString();
        }

        public void speedBar_Scroll(object sender, EventArgs e)
        {
            float value = (float)(speedMax * UV.SpeedBar.Value) / UV.SpeedBar.Maximum;
            myParticle.SpeedFactor = value;
            UV.SpeedBox.Text = myParticle.SpeedFactor.ToString();
        }

        public void checkBoxPoint_CheckedChanged(object sender, EventArgs e)
        {

        }

        public void checkBoxWave_CheckedChanged(object sender, EventArgs e)
        {
            myParticle.IsWave = UV.CheckBoxWave.Checked;
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
            new Tuple<double, uint>(0.0, 0x0021fc),
            new Tuple<double, uint>(0.1, 0x008cd4),
            new Tuple<double, uint>(0.2, 0x00d985),
            new Tuple<double, uint>(0.3, 0x00fe01),
            new Tuple<double, uint>(0.4, 0x74e200),
            new Tuple<double, uint>(0.6, 0xd88600),
            new Tuple<double, uint>(0.8, 0xfe1100),
            new Tuple<double, uint>(1.0, 0xd60089)
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



            int color = UV.ComboBoxColor.SelectedIndex;
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

        public void opacityBar_Scroll(object sender, EventArgs e)
        {
            myParticle.Opacity = opacityMin +
            (float)(UV.OpacityBar.Value * (opacityMax - opacityMin)) / UV.OpacityBar.Maximum;
            UV.OpacityBox.Text = myParticle.Opacity.ToString();
        }

        public void particlesBar_Scroll(object sender, EventArgs e)
        {
            isChangedParticle = true;

            myParticle.updateNumParticles(
                (int)(particleMin + (UV.ParticleBar.Value * (particleMax - particleMin)) / UV.ParticleBar.Maximum)
            );

            UV.ParticlesBox.Text = myParticle.getnumParticles().ToString();

        }

        public void InitSlidesGui(){

            UV.CheckBoxPoint.Checked = myParticle.IsWave;

            //Bar
            UV.ParticleBar.Value = (int)(((myParticle.getnumParticles() - particleMin) * UV.ParticleBar.Maximum) / (particleMax - particleMin));
            UV.FadeBar.Value = (int)(((myParticle.Fade - fadeMin) * UV.FadeBar.Maximum) / (fadeMax - fadeMin));
            UV.OpacityBar.Value = (int)(((myParticle.Opacity - opacityMin) * UV.OpacityBar.Maximum) / (opacityMax - opacityMin));

            UV.SpeedBar.Value = (int)(((myParticle.SpeedFactor - speedMin) * UV.SpeedBar.Maximum) / (speedMax - speedMin));

            //Box
            UV.ParticlesBox.Text = myParticle.getnumParticles().ToString();
            UV.FadeBox.Text = myParticle.Fade.ToString();
            UV.OpacityBox.Text = myParticle.Opacity.ToString();
            UV.SpeedBox.Text = myParticle.SpeedFactor.ToString();

        }

        public bool IsChangedParticle
        {
            get { return isChangedParticle; }
            set { this.isChangedParticle = value; }
        }


    }
}
