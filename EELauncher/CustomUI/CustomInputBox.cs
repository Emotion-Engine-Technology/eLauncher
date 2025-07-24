using System;
using System.Drawing;
using System.Windows.Forms;

namespace EELauncher.CustomUI
{
    public class CustomInputBox : Form
    {
        private Label promptLabel;
        private TextBox inputTextBox;
        private Button okButton;
        private Button cancelButton;

        public string UserInput => inputTextBox.Text;

        public CustomInputBox(string title, string prompt)
        {
            this.Text = title;
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.Size = new Size(400, 180);
            this.BackColor = Color.FromArgb(32, 34, 37); // fundo escuro
            this.Font = new Font("Segoe UI", 10);

            // Label
            promptLabel = new Label()
            {
                Text = prompt,
                ForeColor = Color.White,
                AutoSize = false,
                Size = new Size(360, 40),
                Location = new Point(20, 20),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // TextBox
            inputTextBox = new TextBox()
            {
                Size = new Size(360, 30),
                Location = new Point(20, 65),
                BackColor = Color.FromArgb(50, 53, 59),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // OK Button
            okButton = new Button()
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Location = new Point(220, 110),
                Size = new Size(75, 30),
                BackColor = Color.FromArgb(88, 101, 242),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            okButton.FlatAppearance.BorderSize = 0;
            okButton.Click += (s, e) => this.Close();

            // Cancel Button
            cancelButton = new Button()
            {
                Text = "Cancelar",
                DialogResult = DialogResult.Cancel,
                Location = new Point(305, 110),
                Size = new Size(75, 30),
                BackColor = Color.FromArgb(240, 71, 71),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            cancelButton.FlatAppearance.BorderSize = 0;
            cancelButton.Click += (s, e) => this.Close();

            this.Controls.Add(promptLabel);
            this.Controls.Add(inputTextBox);
            this.Controls.Add(okButton);
            this.Controls.Add(cancelButton);
        }
    }
}
