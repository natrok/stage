using System.Drawing;

namespace ConsoleApp1
{
    partial class UserControl
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
            this.checkBoxWave = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.fadeBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speedBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dropRateBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dropRateBumpBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.regSpeedBar)).BeginInit();
            this.SuspendLayout();
            // 
            // fadeBar
            // 
            this.fadeBar.Location = new System.Drawing.Point(92, 14);
            this.fadeBar.Name = "fadeBar";
            this.fadeBar.Size = new System.Drawing.Size(153, 45);
            this.fadeBar.TabIndex = 0;
            // 
            // speedBar
            // 
            this.speedBar.Location = new System.Drawing.Point(90, 52);
            this.speedBar.Name = "speedBar";
            this.speedBar.Size = new System.Drawing.Size(153, 45);
            this.speedBar.TabIndex = 1;
            // 
            // fadeBox
            // 
            this.fadeBox.Location = new System.Drawing.Point(251, 14);
            this.fadeBox.Name = "fadeBox";
            this.fadeBox.Size = new System.Drawing.Size(39, 20);
            this.fadeBox.TabIndex = 2;
            // 
            // speedBox
            // 
            this.speedBox.Location = new System.Drawing.Point(251, 52);
            this.speedBox.Name = "speedBox";
            this.speedBox.Size = new System.Drawing.Size(39, 20);
            this.speedBox.TabIndex = 3;
            // 
            // dropRateBar
            // 
            this.dropRateBar.Location = new System.Drawing.Point(90, 92);
            this.dropRateBar.Name = "dropRateBar";
            this.dropRateBar.Size = new System.Drawing.Size(153, 45);
            this.dropRateBar.TabIndex = 4;
            // 
            // dropRateBox
            // 
            this.dropRateBox.Location = new System.Drawing.Point(251, 92);
            this.dropRateBox.Name = "dropRateBox";
            this.dropRateBox.Size = new System.Drawing.Size(39, 20);
            this.dropRateBox.TabIndex = 5;
            // 
            // dropRateBumpBox
            // 
            this.dropRateBumpBox.Location = new System.Drawing.Point(251, 126);
            this.dropRateBumpBox.Name = "dropRateBumpBox";
            this.dropRateBumpBox.Size = new System.Drawing.Size(39, 20);
            this.dropRateBumpBox.TabIndex = 6;
            // 
            // dropRateBumpBar
            // 
            this.dropRateBumpBar.Location = new System.Drawing.Point(90, 126);
            this.dropRateBumpBar.Name = "dropRateBumpBar";
            this.dropRateBumpBar.Size = new System.Drawing.Size(153, 45);
            this.dropRateBumpBar.TabIndex = 7;
            // 
            // regSpeedBar
            // 
            this.regSpeedBar.Location = new System.Drawing.Point(90, 159);
            this.regSpeedBar.Name = "regSpeedBar";
            this.regSpeedBar.Size = new System.Drawing.Size(153, 45);
            this.regSpeedBar.TabIndex = 8;
            // 
            // regSpeedBox
            // 
            this.regSpeedBox.Location = new System.Drawing.Point(251, 159);
            this.regSpeedBox.Name = "regSpeedBox";
            this.regSpeedBox.Size = new System.Drawing.Size(39, 20);
            this.regSpeedBox.TabIndex = 9;
            // 
            // lblFade
            // 
            this.lblFade.AutoSize = true;
            this.lblFade.Location = new System.Drawing.Point(17, 21);
            this.lblFade.Name = "lblFade";
            this.lblFade.Size = new System.Drawing.Size(64, 13);
            this.lblFade.TabIndex = 10;
            this.lblFade.Text = "fadeOpacity";
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(17, 59);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(68, 13);
            this.lblSpeed.TabIndex = 11;
            this.lblSpeed.Text = "SpeedFactor";
            // 
            // lblDropRate
            // 
            this.lblDropRate.AutoSize = true;
            this.lblDropRate.Location = new System.Drawing.Point(17, 99);
            this.lblDropRate.Name = "lblDropRate";
            this.lblDropRate.Size = new System.Drawing.Size(51, 13);
            this.lblDropRate.TabIndex = 12;
            this.lblDropRate.Text = "dropRate";
            // 
            // lblDropRateBump
            // 
            this.lblDropRateBump.AutoSize = true;
            this.lblDropRateBump.Location = new System.Drawing.Point(17, 133);
            this.lblDropRateBump.Name = "lblDropRateBump";
            this.lblDropRateBump.Size = new System.Drawing.Size(78, 13);
            this.lblDropRateBump.TabIndex = 13;
            this.lblDropRateBump.Text = "dropRateBump";
            // 
            // lblParticles
            // 
            this.lblParticles.AutoSize = true;
            this.lblParticles.Location = new System.Drawing.Point(17, 166);
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
            this.comboBoxColor.Location = new System.Drawing.Point(183, 200);
            this.comboBoxColor.Name = "comboBoxColor";
            this.comboBoxColor.Size = new System.Drawing.Size(107, 21);
            this.comboBoxColor.TabIndex = 15;
            
            // 
            // checkBoxWave
            // 
            this.checkBoxWave.AutoSize = true;
            this.checkBoxWave.Location = new System.Drawing.Point(16, 200);
            this.checkBoxWave.Name = "checkBoxWave";
            this.checkBoxWave.Size = new System.Drawing.Size(52, 17);
            this.checkBoxWave.TabIndex = 16;
            this.checkBoxWave.Text = "night";
            this.checkBoxWave.UseVisualStyleBackColor = true;
            // 
            // UserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(26)))), ((int)(((byte)(26)))), ((int)(((byte)(26)))));
            this.Controls.Add(this.checkBoxWave);
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
            this.Name = "UserControl";
            this.Size = new System.Drawing.Size(316, 238);
            ((System.ComponentModel.ISupportInitialize)(this.fadeBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speedBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dropRateBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dropRateBumpBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.regSpeedBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar fadeBar;
        private System.Windows.Forms.TrackBar speedBar;
        private System.Windows.Forms.TrackBar dropRateBar;
        private System.Windows.Forms.TrackBar dropRateBumpBar;
        private System.Windows.Forms.TrackBar regSpeedBar;

        private System.Windows.Forms.TextBox fadeBox;
        private System.Windows.Forms.TextBox speedBox;
        private System.Windows.Forms.TextBox dropRateBox;
        private System.Windows.Forms.TextBox dropRateBumpBox;
        private System.Windows.Forms.TextBox regSpeedBox;

        
        private System.Windows.Forms.Label lblFade;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.Label lblDropRate;
        private System.Windows.Forms.Label lblDropRateBump;
        private System.Windows.Forms.Label lblParticles;
        private System.Windows.Forms.ComboBox comboBoxColor;
        private System.Windows.Forms.CheckBox checkBoxWave;
    }
}
