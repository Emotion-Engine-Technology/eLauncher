namespace EELauncher
{
    partial class ELLoading
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ELLoading));
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges3 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            Guna.UI2.WinForms.Suite.CustomizableEdges customizableEdges4 = new Guna.UI2.WinForms.Suite.CustomizableEdges();
            pictureBox1 = new PictureBox();
            LoadingProgressBar = new Guna.UI2.WinForms.Guna2ProgressBar();
            label13 = new Label();
            label1 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // pictureBox1
            // 
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(-1, 30);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(234, 226);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 5;
            pictureBox1.TabStop = false;
            // 
            // LoadingProgressBar
            // 
            LoadingProgressBar.CustomizableEdges = customizableEdges3;
            LoadingProgressBar.FillColor = Color.FromArgb(28, 28, 41);
            LoadingProgressBar.ForeColor = Color.DimGray;
            LoadingProgressBar.Location = new Point(-1, 302);
            LoadingProgressBar.Name = "LoadingProgressBar";
            LoadingProgressBar.ProgressColor = Color.White;
            LoadingProgressBar.ProgressColor2 = Color.White;
            LoadingProgressBar.ShadowDecoration.CustomizableEdges = customizableEdges4;
            LoadingProgressBar.Size = new Size(234, 18);
            LoadingProgressBar.TabIndex = 1;
            LoadingProgressBar.Text = "LoadingProgressBar";
            LoadingProgressBar.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.BackColor = Color.Transparent;
            label13.Font = new Font("Segoe UI Semibold", 17.25F, FontStyle.Bold);
            label13.ForeColor = Color.White;
            label13.Location = new Point(53, 225);
            label13.Name = "label13";
            label13.Size = new Size(120, 31);
            label13.TabIndex = 37;
            label13.Text = "eLauncher";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Segoe UI Semibold", 9.25F, FontStyle.Bold);
            label1.ForeColor = Color.White;
            label1.Location = new Point(59, 259);
            label1.Name = "label1";
            label1.Size = new Size(110, 17);
            label1.TabIndex = 38;
            label1.Text = "SA-MP and OMP";
            // 
            // ELLoading
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(28, 28, 41);
            ClientSize = new Size(233, 320);
            Controls.Add(label1);
            Controls.Add(label13);
            Controls.Add(LoadingProgressBar);
            Controls.Add(pictureBox1);
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "ELLoading";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Emotion Launcher";
            Load += Form1_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private PictureBox pictureBox1;
        private Guna.UI2.WinForms.Guna2ProgressBar LoadingProgressBar;
        private Label label13;
        private Label label1;
    }
}
