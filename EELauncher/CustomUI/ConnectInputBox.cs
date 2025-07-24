using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace EELauncher.CustomUI
{
    public class ConnectInputBox : Form
    {
        private Label ServerLabel;
        private Label NicknameLabel;
        private TextBox nicknameTextBox;
        private Label PasswordLabel;
        private TextBox PasswordBox;
        private Button ConfirmButton;
        private Button CancelButton;

        private string iniPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.ini");

        public string UserNickname => nicknameTextBox.Text.Trim();
        public string ServerPassword => PasswordBox.Text;

        public ConnectInputBox(string serverIp, string serverPort, bool requiresPassword)
        {
            // Configurações básicas do formulário
            this.Text = "Conectar ao Servidor";
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(360, 220);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.ShowInTaskbar = false;
            this.BackColor = Color.FromArgb(40, 40, 40);
            this.ForeColor = Color.White;

            // Label do servidor
            ServerLabel = new Label()
            {
                Text = $"Conectando em {serverIp}:{serverPort}",
                Location = new Point(15, 15),
                AutoSize = true,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.LightGreen,
                BackColor = Color.Transparent
            };
            this.Controls.Add(ServerLabel);

            // Label Nickname
            NicknameLabel = new Label()
            {
                Text = "Nickname:",
                Location = new Point(15, 55),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.Gainsboro,
                BackColor = Color.Transparent
            };
            this.Controls.Add(NicknameLabel);

            // TextBox Nickname
            nicknameTextBox = new TextBox()
            {
                Location = new Point(15, 75),
                Width = 320,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10),
            };
            this.Controls.Add(nicknameTextBox);

            // Label Senha
            PasswordLabel = new Label()
            {
                Text = "Server Password(Optional):",
                Location = new Point(15, 115),
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.Gainsboro,
                BackColor = Color.Transparent
            };
            this.Controls.Add(PasswordLabel);

            // TextBox Senha
            PasswordBox = new TextBox()
            {
                Location = new Point(15, 135),
                Width = 320,
                UseSystemPasswordChar = true,
                BackColor = Color.FromArgb(45, 45, 45),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10),
                Visible = true
            };
            this.Controls.Add(PasswordBox);

            // Botão Confirmar
            ConfirmButton = new Button()
            {
                Text = "Join",
                Size = new Size(110, 35),
                Location = new Point(70, 175),
                BackColor = Color.FromArgb(0, 180, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                DialogResult = DialogResult.OK
            };
            ConfirmButton.FlatAppearance.BorderSize = 0;
            ConfirmButton.Click += ConfirmButton_Click;
            this.Controls.Add(ConfirmButton);

            // Botão Cancelar
            CancelButton = new Button()
            {
                Text = "Cancel",
                Size = new Size(110, 35),
                Location = new Point(190, 175),
                BackColor = Color.FromArgb(200, 30, 30),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                DialogResult = DialogResult.Cancel
            };
            CancelButton.FlatAppearance.BorderSize = 0;
            this.Controls.Add(CancelButton);

            // Define Enter e Esc
            this.AcceptButton = ConfirmButton;
            this.CancelButton = CancelButton;

            // Carregar nickname salvo no arquivo
            LoadNicknameFromIni();
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserNickname))
            {
                MessageBox.Show("Por favor, insira um nickname válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None; // não fecha o form
                return;
            }

            SaveNicknameToIni(UserNickname);
            // Senha é opcional, não valida
        }

        private void LoadNicknameFromIni()
        {
            try
            {
                if (File.Exists(iniPath))
                {
                    var lines = File.ReadAllLines(iniPath);
                    foreach (var line in lines)
                    {
                        if (line.StartsWith("nickname="))
                        {
                            nicknameTextBox.Text = line.Substring("nickname=".Length);
                            break;
                        }
                    }
                }
            }
            catch
            {
                // Ignorar erros
            }
        }

        private void SaveNicknameToIni(string nickname)
        {
            try
            {
                File.WriteAllText(iniPath, $"nickname={nickname}");
            }
            catch
            {
                // Ignorar erros
            }
        }
    }
}
