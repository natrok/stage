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
    public partial class UserView : System.Windows.Forms.UserControl
    {
        public UserView()
        {
            InitializeComponent();
            fadeBar.Minimum = 0;
            fadeBar.Maximum = 100;
            speedBar.Minimum = 0;
            speedBar.Maximum = 100;

            particlesBar.Minimum = 0;
            particlesBar.Maximum = 10000;
            opacityBar.Minimum = 0;
            opacityBar.Maximum = 100;

            checkBoxPoint.Checked = false;
            checkBoxWave.Checked = false;
            comboBoxColor.SelectedIndex = 3;

        

        }

        public TextBox FadeBox
        {
            get { return fadeBox; }          
        }

        public TrackBar FadeBar
        {
            get { return fadeBar; }
            set { this.fadeBar = value; }
        }


        public TextBox SpeedBox
        {
            get { return speedBox; }
        }

        public TrackBar SpeedBar
        {
            get { return speedBar; }
        }

        public TextBox OpacityBox
        {
            get { return opacityBox; }
        }

        public TrackBar OpacityBar
        {
            get { return opacityBar; }
        }

        public TextBox ParticlesBox
        {
            get { return particlesBox; }
        }

        public TrackBar ParticleBar
        {
            get { return particlesBar; }
        }
        
        public CheckBox CheckBoxPoint
        {
            get { return checkBoxPoint; }
        }

        public CheckBox CheckBoxWave
        {
            get { return checkBoxWave; }
        }

        public ComboBox ComboBoxColor
        {
            get { return comboBoxColor; }
        }

        public void setControl(ControlSettings obj)
        {
            this.fadeBar.Scroll += new System.EventHandler(obj.fadeBar_Scroll);
            this.speedBar.Scroll += new System.EventHandler(obj.speedBar_Scroll);
            this.particlesBar.Scroll += new System.EventHandler(obj.particlesBar_Scroll);
            this.opacityBar.Scroll += new System.EventHandler(obj.opacityBar_Scroll);

            this.checkBoxPoint.CheckedChanged += new System.EventHandler(obj.checkBoxPoint_CheckedChanged);
            this.checkBoxWave.CheckedChanged += new System.EventHandler(obj.checkBoxWave_CheckedChanged);

            this.comboBoxColor.SelectedIndexChanged += new System.EventHandler(obj.comboBoxColor_SelectedIndexChanged);           

        }


    }
}
