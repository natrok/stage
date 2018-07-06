﻿using System;
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
            dropRateBar.Minimum = 0;
            dropRateBar.Maximum = 1000;
            dropRateBumpBar.Minimum = 0;
            dropRateBumpBar.Maximum = 1000;
            regSpeedBar.Minimum = 0;
            regSpeedBar.Maximum = 100;

            particlesBar.Minimum = 0;
            particlesBar.Maximum = 10000;
            opacityBar.Minimum = 0;
            opacityBar.Maximum = 100;

            checkBoxPoint.Checked = false;
            checkBoxWave.Checked = false;
            comboBoxColor.SelectedIndex = 3;

        

        }

        public TextBox FadeBox{
            get { return fadeBox; }          
        }

        public TrackBar FadeBar {
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


        public TextBox RegSpeedBox
        {
            get { return regSpeedBox; }
        }

        public TrackBar RegSpeedBar
        {
            get { return regSpeedBar; }
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

        public void setControl(ControlSettings obj) {
            this.fadeBar.Scroll += new System.EventHandler(obj.fadeBar_Scroll);
            this.speedBar.Scroll += new System.EventHandler(obj.speedBar_Scroll);
            this.regSpeedBar.Scroll += new System.EventHandler(obj.speedRegBar_Scroll);
            this.dropRateBar.Scroll += new System.EventHandler(obj.dropRate_Scroll);
            this.DropRateBumpBar.Scroll += new System.EventHandler(obj.dropRateBump_Scroll);
            this.particlesBar.Scroll += new System.EventHandler(obj.particlesBar_Scroll);
            this.opacityBar.Scroll += new System.EventHandler(obj.opacityBar_Scroll);

            this.checkBoxPoint.CheckedChanged += new System.EventHandler(obj.checkBoxPoint_CheckedChanged);
            this.checkBoxWave.CheckedChanged += new System.EventHandler(obj.checkBoxWave_CheckedChanged);
            this.comboBoxColor.SelectedIndexChanged += new System.EventHandler(obj.comboBoxColor_SelectedIndexChanged);

            

        }

    }
}
