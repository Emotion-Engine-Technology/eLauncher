using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Guna.UI2.WinForms;

namespace EELauncher.UI
{
    public class CustomMessageBox : Form
    {
        // Import para bordas arredondadas
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        private Guna2Button btnOk;
        private Label lblMessage;
        private Label lblTitle;

        public CustomMessageBox(string title, string message, string buttonText = "OK")
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(400, 200);
            this.BackColor = Color.FromArgb(28, 28, 56);
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            lblTitle = new Label
            {
                Text = title,
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(20, 20),
                AutoSize = true
            };

            lblMessage = new Label
            {
                Text = message,
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gainsboro,
                Location = new Point(20, 60),
                Size = new Size(360, 60)
            };

            btnOk = new Guna2Button
            {
                Text = buttonText,
                DialogResult = DialogResult.OK,
                Size = new Size(100, 35),
                Location = new Point((this.Width - 100) / 2, 135),
                FillColor = Color.FromArgb(0, 120, 215),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                BorderRadius = 8
            };

            btnOk.Click += (s, e) => this.Close();

            Controls.Add(lblTitle);
            Controls.Add(lblMessage);
            Controls.Add(btnOk);
        }

        public static void Show(string title, string message, string buttonText = "OK", Form parent = null)
        {
            using (var box = new CustomMessageBox(title, message, buttonText))
            {
                if (parent != null)
                    box.ShowDialog(parent);
                else
                    box.ShowDialog();
            }
        }
    }
}
