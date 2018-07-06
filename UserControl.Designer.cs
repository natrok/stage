using System.Drawing;

namespace ConsoleApp1
{
    partial class UserView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.fadeBar = new System.Windows.Forms.TrackBar();
            this.speedBar = new System.Windows.Forms.TrackBar();
            this.fadeBox = new System.Windows.Forms.TextBox();
            this.speedBox = new System.Windows.Forms.TextBox();
            this.dropRateBar = new System.Windows.Forms.TrackBar();
            this.dropRateBox = new System.Windows.Forms.TextBox();
            this.dropRateBumpBox = new System.Windows.Forms.TextBox();
            this.dropRateBumpBar = new System.Windows.Forms.TrackBar();
            this.regSpeedBar = new System.Windows.Forms.TrackBar();
            this.regSpeedBox = new System.Windows.Forms.TextBox();
            this.lblFade = new System.Windows.Forms.Label();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.lblDropRate = new System.Windows.Forms.Label();
            this.lblDropRateBump = new System.Windows.Forms.Label();
            this.lblParticles = new System.Windows.Forms.Label();
            this.comboBoxColor = new System.Windows.Forms.ComboBox();
            this.checkBoxPoint = new System.Windows.Forms.CheckBox();
            this.checkBoxWave = new System.Windows.Forms.CheckBox();
            this.particlesBox = new System.Windows.Forms.TextBox();
            this.particlesBar = new System.Windows.Forms.TrackBar();
            this.lblNumParticles = new System.Windows.Forms.Label();
            this.opacityBox = new System.Windows.Forms.TextBox();
            this.opacityBar = new System.Windows.Forms.TrackBar();
            this.lblOpacity = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.fadeBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speedBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dropRateBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dropRateBumpBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.regSpeedBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.particlesBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.opacityBar)).BeginInit();
            this.SuspendLayout();
            // 
            // fadeBar
            // 
            this.fadeBar.Location = new System.Drawing.Point(89, 82);
            this.fadeBar.Name = "fadeBar";
            this.fadeBar.Size = new System.Drawing.Size(153, 45);
            this.fadeBar.TabIndex = 0;
            // 
            // speedBar
            // 
            this.speedBar.Location = new System.Drawing.Point(89, 117);
            this.speedBar.Name = "speedBar";
            this.speedBar.Size = new System.Drawing.Size(153, 45);
            this.speedBar.TabIndex = 1;
            // 
            // fadeBox
            // 
            this.fadeBox.Location = new System.Drawing.Point(250, 79);
            this.fadeBox.Name = "fadeBox";
            this.fadeBox.Size = new System.Drawing.Size(39, 20);
            this.fadeBox.TabIndex = 2;
            // 
            // speedBox
            // 
            this.speedBox.Location = new System.Drawing.Point(250, 117);
            this.speedBox.Name = "speedBox";
            this.speedBox.Size = new System.Drawing.Size(39, 20);
            this.speedBox.TabIndex = 3;
            // 
            // dropRateBar
            // 
            this.dropRateBar.Location = new System.Drawing.Point(89, 157);
            this.dropRateBar.Name = "dropRateBar";
            this.dropRateBar.Size = new System.Drawing.Size(153, 45);
            this.dropRateBar.TabIndex = 4;
            // 
            // dropRateBox
            // 
            this.dropRateBox.Location = new System.Drawing.Point(250, 157);
            this.dropRateBox.Name = "dropRateBox";
            this.dropRateBox.Size = new System.Drawing.Size(39, 20);
            this.dropRateBox.TabIndex = 5;
            // 
            // dropRateBumpBox
            // 
            this.dropRateBumpBox.Location = new System.Drawing.Point(250, 196);
            this.dropRateBumpBox.Name = "dropRateBumpBox";
            this.dropRateBumpBox.Size = new System.Drawing.Size(39, 20);
            this.dropRateBumpBox.TabIndex = 6;
            // 
            // dropRateBumpBar
            // 
            this.dropRateBumpBar.Location = new System.Drawing.Point(89, 196);
            this.dropRateBumpBar.Name = "dropRateBumpBar";
            this.dropRateBumpBar.Size = new System.Drawing.Size(153, 45);
            this.dropRateBumpBar.TabIndex = 7;
            // 
            // regSpeedBar
            // 
            this.regSpeedBar.Location = new System.Drawing.Point(89, 238);
            this.regSpeedBar.Name = "regSpeedBar";
            this.regSpeedBar.Size = new System.Drawing.Size(153, 45);
            this.regSpeedBar.TabIndex = 8;
            // 
            // regSpeedBox
            // 
            this.regSpeedBox.Location = new System.Drawing.Point(250, 238);
            this.regSpeedBox.Name = "regSpeedBox";
            this.regSpeedBox.Size = new System.Drawing.Size(39, 20);
            this.regSpeedBox.TabIndex = 9;
            // 
            // lblFade
            // 
            this.lblFade.AutoSize = true;
            this.lblFade.Location = new System.Drawing.Point(15, 82);
            this.lblFade.Name = "lblFade";
            this.lblFade.Size = new System.Drawing.Size(28, 13);
            this.lblFade.TabIndex = 10;
            this.lblFade.Text = "fade";
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(15, 120);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(68, 13);
            this.lblSpeed.TabIndex = 11;
            this.lblSpeed.Text = "SpeedFactor";
            // 
            // lblDropRate
            // 
            this.lblDropRate.AutoSize = true;
            this.lblDropRate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(140)))), ((int)(((byte)(47)))));
            this.lblDropRate.Location = new System.Drawing.Point(15, 160);
            this.lblDropRate.Name = "lblDropRate";
            this.lblDropRate.Size = new System.Drawing.Size(51, 13);
            this.lblDropRate.TabIndex = 12;
            this.lblDropRate.Text = "dropRate";
            // 
            // lblDropRateBump
            // 
            this.lblDropRateBump.AutoSize = true;
            this.lblDropRateBump.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(140)))), ((int)(((byte)(47)))));
            this.lblDropRateBump.Location = new System.Drawing.Point(15, 199);
            this.lblDropRateBump.Name = "lblDropRateBump";
            this.lblDropRateBump.Size = new System.Drawing.Size(78, 13);
            this.lblDropRateBump.TabIndex = 13;
            this.lblDropRateBump.Text = "dropRateBump";
            // 
            // lblParticles
            // 
            this.lblParticles.AutoSize = true;
            this.lblParticles.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(140)))), ((int)(((byte)(47)))));
            this.lblParticles.Location = new System.Drawing.Point(15, 241);
            this.lblParticles.Name = "lblParticles";
            this.lblParticles.Size = new System.Drawing.Size(53, 13);
            this.lblParticles.TabIndex = 14;
            this.lblParticles.Text = "regSpeed";
            // 
            // comboBoxColor
            // 
            this.comboBoxColor.FormattingEnabled = true;
            this.comboBoxColor.Items.AddRange(new object[] {
            "Default",
            "Cool",
            "Hot",
            "Gris"});
            this.comboBoxColor.Location = new System.Drawing.Point(182, 288);
            this.comboBoxColor.Name = "comboBoxColor";
            this.comboBoxColor.Size = new System.Drawing.Size(107, 21);
            this.comboBoxColor.TabIndex = 15;
            // 
            // checkBoxPoint
            // 
            this.checkBoxPoint.AutoSize = true;
            this.checkBoxPoint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(140)))), ((int)(((byte)(47)))));
            this.checkBoxPoint.Location = new System.Drawing.Point(18, 269);
            this.checkBoxPoint.Name = "checkBoxPoint";
            this.checkBoxPoint.Size = new System.Drawing.Size(42, 17);
            this.checkBoxPoint.TabIndex = 16;
            this.checkBoxPoint.Text = "Old";
            this.checkBoxPoint.UseVisualStyleBackColor = true;
            // 
            // checkBoxWave
            // 
            this.checkBoxWave.AutoSize = true;
            this.checkBoxWave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(241)))), ((int)(((byte)(214)))));
            this.checkBoxWave.Location = new System.Drawing.Point(18, 292);
            this.checkBoxWave.Name = "checkBoxWave";
            this.checkBoxWave.Size = new System.Drawing.Size(52, 17);
            this.checkBoxWave.TabIndex = 17;
            this.checkBoxWave.Text = "wave";
            this.checkBoxWave.UseVisualStyleBackColor = true;
            // 
            // particlesBox
            // 
            this.particlesBox.Location = new System.Drawing.Point(250, 3);
            this.particlesBox.Name = "particlesBox";
            this.particlesBox.Size = new System.Drawing.Size(39, 20);
            this.particlesBox.TabIndex = 18;
            // 
            // particlesBar
            // 
            this.particlesBar.Location = new System.Drawing.Point(89, 3);
            this.particlesBar.Name = "particlesBar";
            this.particlesBar.Size = new System.Drawing.Size(153, 45);
            this.particlesBar.TabIndex = 19;
            // 
            // lblNumParticles
            // 
            this.lblNumParticles.AutoSize = true;
            this.lblNumParticles.Location = new System.Drawing.Point(15, 10);
            this.lblNumParticles.Name = "lblNumParticles";
            this.lblNumParticles.Size = new System.Drawing.Size(75, 13);
            this.lblNumParticles.TabIndex = 20;
            this.lblNumParticles.Text = "Num. Particles";
            // 
            // opacityBox
            // 
            this.opacityBox.Location = new System.Drawing.Point(250, 38);
            this.opacityBox.Name = "opacityBox";
            this.opacityBox.Size = new System.Drawing.Size(39, 20);
            this.opacityBox.TabIndex = 21;
            // 
            // opacityBar
            // 
            this.opacityBar.Location = new System.Drawing.Point(89, 38);
            this.opacityBar.Name = "opacityBar";
            this.opacityBar.Size = new System.Drawing.Size(153, 45);
            this.opacityBar.TabIndex = 22;
            // 
            // lblOpacity
            // 
            this.lblOpacity.AutoSize = true;
            this.lblOpacity.Location = new System.Drawing.Point(15, 38);
            this.lblOpacity.Name = "lblOpacity";
            this.lblOpacity.Size = new System.Drawing.Size(43, 13);
            this.lblOpacity.TabIndex = 23;
            this.lblOpacity.Text = "Opacity";
            // 
            // UserView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.Controls.Add(this.lblOpacity);
            this.Controls.Add(this.opacityBar);
            this.Controls.Add(this.opacityBox);
            this.Controls.Add(this.lblNumParticles);
            this.Controls.Add(this.particlesBar);
            this.Controls.Add(this.particlesBox);
            this.Controls.Add(this.checkBoxWave);
            this.Controls.Add(this.checkBoxPoint);
            this.Controls.Add(this.comboBoxColor);
            this.Controls.Add(this.lblParticles);
            this.Controls.Add(this.lblDropRateBump);
            this.Controls.Add(this.lblDropRate);
            this.Controls.Add(this.lblSpeed);
            this.Controls.Add(this.lblFade);
            this.Controls.Add(this.regSpeedBox);
            this.Controls.Add(this.regSpeedBar);
            this.Controls.Add(this.dropRateBumpBar);
            this.Controls.Add(this.dropRateBumpBox);
            this.Controls.Add(this.dropRateBox);
            this.Controls.Add(this.dropRateBar);
            this.Controls.Add(this.speedBox);
            this.Controls.Add(this.fadeBox);
            this.Controls.Add(this.speedBar);
            this.Controls.Add(this.fadeBar);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(161)))), ((int)(((byte)(214)))));
            this.Name = "UserView";
            this.Size = new System.Drawing.Size(316, 316);
            ((System.ComponentModel.ISupportInitialize)(this.fadeBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speedBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dropRateBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dropRateBumpBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.regSpeedBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.particlesBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.opacityBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar fadeBar;
        private System.Windows.Forms.TrackBar speedBar;
        private System.Windows.Forms.TrackBar dropRateBar;
        private System.Windows.Forms.TrackBar dropRateBumpBar;
        private System.Windows.Forms.TrackBar regSpeedBar;
        private System.Windows.Forms.TrackBar opacityBar;
        private System.Windows.Forms.TrackBar particlesBar;

        private System.Windows.Forms.TextBox fadeBox;
        private System.Windows.Forms.TextBox speedBox;
        private System.Windows.Forms.TextBox dropRateBox;
        private System.Windows.Forms.TextBox dropRateBumpBox;
        private System.Windows.Forms.TextBox regSpeedBox;
        private System.Windows.Forms.TextBox opacityBox;
        private System.Windows.Forms.TextBox particlesBox;

        private System.Windows.Forms.Label lblFade;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.Label lblDropRate;
        private System.Windows.Forms.Label lblDropRateBump;
        private System.Windows.Forms.Label lblParticles;
        private System.Windows.Forms.Label lblNumParticles;
        private System.Windows.Forms.Label lblOpacity;

        private System.Windows.Forms.ComboBox comboBoxColor;

        private System.Windows.Forms.CheckBox checkBoxPoint;
        private System.Windows.Forms.CheckBox checkBoxWave;






    }
}
