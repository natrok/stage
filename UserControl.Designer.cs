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
            this.lblFade = new System.Windows.Forms.Label();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.comboBoxColor = new System.Windows.Forms.ComboBox();
            this.checkBoxPoint = new System.Windows.Forms.CheckBox();
            this.checkBoxWave = new System.Windows.Forms.CheckBox();
            this.particlesBar = new System.Windows.Forms.TrackBar();
            this.lblNumParticles = new System.Windows.Forms.Label();
            this.opacityBox = new System.Windows.Forms.TextBox();
            this.opacityBar = new System.Windows.Forms.TrackBar();
            this.lblOpacity = new System.Windows.Forms.Label();
            this.particlesBox = new System.Windows.Forms.TextBox();

            ((System.ComponentModel.ISupportInitialize)(this.fadeBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.speedBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.particlesBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.opacityBar)).BeginInit();
            this.SuspendLayout();
            // 
            // fadeBar
            // 
            this.fadeBar.Location = new System.Drawing.Point(89, 78);
            this.fadeBar.Name = "fadeBar";
            this.fadeBar.Size = new System.Drawing.Size(118, 45);
            this.fadeBar.TabIndex = 0;
            // 
            // speedBar
            // 
            this.speedBar.Location = new System.Drawing.Point(89, 113);
            this.speedBar.Name = "speedBar";
            this.speedBar.Size = new System.Drawing.Size(118, 45);
            this.speedBar.TabIndex = 1;
            // 
            // fadeBox
            // 
            this.fadeBox.Location = new System.Drawing.Point(213, 78);
            this.fadeBox.Name = "fadeBox";
            this.fadeBox.Size = new System.Drawing.Size(39, 20);
            this.fadeBox.TabIndex = 2;
            // 
            // speedBox
            // 
            this.speedBox.Location = new System.Drawing.Point(213, 116);
            this.speedBox.Name = "speedBox";
            this.speedBox.Size = new System.Drawing.Size(39, 20);
            this.speedBox.TabIndex = 3;
            // 
            // lblFade
            // 
            this.lblFade.AutoSize = true;
            this.lblFade.Location = new System.Drawing.Point(15, 78);
            this.lblFade.Name = "lblFade";
            this.lblFade.Size = new System.Drawing.Size(28, 13);
            this.lblFade.TabIndex = 10;
            this.lblFade.Text = "fade";
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(15, 116);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(68, 13);
            this.lblSpeed.TabIndex = 11;
            this.lblSpeed.Text = "SpeedFactor";
            // 
            // particlesBox
            // 
            particlesBox.BackColor = System.Drawing.Color.White;
            particlesBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            particlesBox.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(161)))), ((int)(((byte)(214)))));
            particlesBox.Location = new System.Drawing.Point(213, 3);
            particlesBox.Name = "particlesBox";
            particlesBox.Size = new System.Drawing.Size(39, 20);
            particlesBox.TabIndex = 18;
            // 
            // comboBoxColor
            // 
            this.comboBoxColor.FormattingEnabled = true;
            this.comboBoxColor.Items.AddRange(new object[] {
            "Default",
            "Cool",
            "Hot",
            "Gris"});
            this.comboBoxColor.Location = new System.Drawing.Point(145, 164);
            this.comboBoxColor.Name = "comboBoxColor";
            this.comboBoxColor.Size = new System.Drawing.Size(107, 21);
            this.comboBoxColor.TabIndex = 15;
            // 
            // checkBoxPoint
            // 
            this.checkBoxPoint.AutoSize = true;
            this.checkBoxPoint.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(240)))), ((int)(((byte)(214)))));
            this.checkBoxPoint.Location = new System.Drawing.Point(18, 149);
            this.checkBoxPoint.Name = "checkBoxPoint";
            this.checkBoxPoint.Size = new System.Drawing.Size(74, 17);
            this.checkBoxPoint.TabIndex = 16;
            this.checkBoxPoint.Text = "Color Map";
            this.checkBoxPoint.UseVisualStyleBackColor = true;
            // 
            // checkBoxWave
            // 
            this.checkBoxWave.AutoSize = true;
            this.checkBoxWave.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(241)))), ((int)(((byte)(214)))));
            this.checkBoxWave.Location = new System.Drawing.Point(18, 172);
            this.checkBoxWave.Name = "checkBoxWave";
            this.checkBoxWave.Size = new System.Drawing.Size(52, 17);
            this.checkBoxWave.TabIndex = 17;
            this.checkBoxWave.Text = "wave";
            this.checkBoxWave.UseVisualStyleBackColor = true;
            // 
            // particlesBar
            // 
            this.particlesBar.Location = new System.Drawing.Point(89, 3);
            this.particlesBar.Name = "particlesBar";
            this.particlesBar.Size = new System.Drawing.Size(118, 45);
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
            this.opacityBox.Location = new System.Drawing.Point(213, 38);
            this.opacityBox.Name = "opacityBox";
            this.opacityBox.Size = new System.Drawing.Size(39, 20);
            this.opacityBox.TabIndex = 21;
            // 
            // opacityBar
            // 
            this.opacityBar.Location = new System.Drawing.Point(89, 38);
            this.opacityBar.Name = "opacityBar";
            this.opacityBar.Size = new System.Drawing.Size(118, 45);
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
            this.Controls.Add(particlesBox);
            this.Controls.Add(this.checkBoxWave);
            this.Controls.Add(this.checkBoxPoint);
            this.Controls.Add(this.comboBoxColor);
            this.Controls.Add(this.lblSpeed);
            this.Controls.Add(this.lblFade);
            this.Controls.Add(this.speedBox);
            this.Controls.Add(this.fadeBox);
            this.Controls.Add(this.speedBar);
            this.Controls.Add(this.fadeBar);
            this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(161)))), ((int)(((byte)(214)))));
            this.Name = "UserView";
            this.Size = new System.Drawing.Size(270, 202);
            ((System.ComponentModel.ISupportInitialize)(this.fadeBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.speedBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.particlesBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.opacityBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar fadeBar;
        private System.Windows.Forms.TrackBar speedBar;
        private System.Windows.Forms.TrackBar opacityBar;
        private System.Windows.Forms.TrackBar particlesBar;

        private System.Windows.Forms.TextBox fadeBox;
        private System.Windows.Forms.TextBox speedBox;
        private System.Windows.Forms.TextBox opacityBox;
        private System.Windows.Forms.TextBox particlesBox;


        private System.Windows.Forms.Label lblFade;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.Label lblNumParticles;
        private System.Windows.Forms.Label lblOpacity;

        private System.Windows.Forms.ComboBox comboBoxColor;

        private System.Windows.Forms.CheckBox checkBoxPoint;
        private System.Windows.Forms.CheckBox checkBoxWave;






    }
}
