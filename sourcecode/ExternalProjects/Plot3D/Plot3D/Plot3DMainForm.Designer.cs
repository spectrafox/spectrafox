namespace Plot3D
{
    partial class Plot3DMainForm
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
            this.tbHue = new System.Windows.Forms.TrackBar();
            ((System.ComponentModel.ISupportInitialize)(this.tbHue)).BeginInit();
            this.SuspendLayout();
            // 
            // tbHue
            // 
            this.tbHue.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tbHue.Location = new System.Drawing.Point(0, 481);
            this.tbHue.Maximum = 360;
            this.tbHue.Name = "tbHue";
            this.tbHue.Size = new System.Drawing.Size(536, 45);
            this.tbHue.TabIndex = 0;
            this.tbHue.TickFrequency = 20;
            this.tbHue.TickStyle = System.Windows.Forms.TickStyle.None;
            this.tbHue.Value = 10;
            this.tbHue.Scroll += new System.EventHandler(this.tbHue_Scroll);
            // 
            // Plot3DMainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(536, 526);
            this.Controls.Add(this.tbHue);
            this.Name = "Plot3DMainForm";
            this.Text = "Plot 3D surface";
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.tbHue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar tbHue;

    }
}

