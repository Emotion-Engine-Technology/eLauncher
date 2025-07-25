using System;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using EELauncher;
using SAMPQuery;

namespace EELauncher.CustomUI
{
    public class ConnectInputBox : Form
    {
        private EEL mainForm;
        private Label ServerLabel;
        private Label NameServerLabel;
        private Label PlayersServerLabel;
        private Label NicknameLabel;
        private TextBox nicknameTextBox;
        private Label PasswordLabel;
        private TextBox PasswordBox;
        private Button ConfirmButton;
        private Button CancelButton;

        private string iniPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.ini");

        public string UserNickname => nicknameTextBox.Text.Trim();
        public string ServerPassword => PasswordBox.Text;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        public ConnectInputBox(string serverIp, string serverPort, bool requiresPassword)
        {
            this.mainForm = mainForm;

            this.Text = "Conectar ao Servidor";
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterParent;
            this.ClientSize = new Size(380, 240);
            this.BackColor = Color.FromArgb(28, 28, 56);
            this.ForeColor = Color.White;
            this.DoubleBuffered = true;

            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            ServerLabel = new Label()
            {
                Text = $"Conectando em {serverIp}:{serverPort}",
                Location = new Point(20, 10),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 11, FontStyle.Bold),
                ForeColor = Color.LightGreen,
                BackColor = Color.Transparent
            };
            this.Controls.Add(ServerLabel);

            NameServerLabel = new Label()
            {
                Text = "Nome do servidor...",
                Location = new Point(20, 35),
                AutoSize = true,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gainsboro,
                BackColor = Color.Transparent
            };
            this.Controls.Add(NameServerLabel);

            PlayersServerLabel = new Label()
            {
                Text = "Jogadores: ...",
                Location = new Point(20, 55),
                AutoSize = true,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gainsboro,
                BackColor = Color.Transparent
            };
            this.Controls.Add(PlayersServerLabel);

            NicknameLabel = new Label()
            {
                Text = "Nickname:",
                Location = new Point(20, 85),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9),
                ForeColor = Color.Gainsboro
            };
            this.Controls.Add(NicknameLabel);

            nicknameTextBox = new TextBox()
            {
                Location = new Point(20, 105),
                Width = 340,
                BackColor = Color.FromArgb(45, 45, 65),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10),
            };
            this.Controls.Add(nicknameTextBox);

            PasswordLabel = new Label()
            {
                Text = "Senha do servidor (opcional):",
                Location = new Point(20, 140),
                AutoSize = true,
                Font = new Font("Segoe UI Semibold", 9),
                ForeColor = Color.Gainsboro
            };
            this.Controls.Add(PasswordLabel);

            PasswordBox = new TextBox()
            {
                Location = new Point(20, 160),
                Width = 340,
                UseSystemPasswordChar = true,
                BackColor = Color.FromArgb(45, 45, 65),
                ForeColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                Font = new Font("Segoe UI", 10),
            };
            this.Controls.Add(PasswordBox);

            ConfirmButton = new Button()
            {
                Text = "Entrar",
                Size = new Size(120, 36),
                Location = new Point(60, 200),
                BackColor = Color.FromArgb(0, 180, 80),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                DialogResult = DialogResult.OK
            };
            ConfirmButton.FlatAppearance.BorderSize = 0;
            ConfirmButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 200, 100);
            ConfirmButton.Click += ConfirmButton_Click;
            this.Controls.Add(ConfirmButton);

            CancelButton = new Button()
            {
                Text = "Cancelar",
                Size = new Size(120, 36),
                Location = new Point(200, 200),
                BackColor = Color.FromArgb(180, 50, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                DialogResult = DialogResult.Cancel
            };
            CancelButton.FlatAppearance.BorderSize = 0;
            CancelButton.FlatAppearance.MouseOverBackColor = Color.FromArgb(200, 70, 70);
            this.Controls.Add(CancelButton);

            this.AcceptButton = ConfirmButton;
            this.CancelButton = CancelButton;

            LoadNicknameFromIni();
            _ = LoadServerInfo(serverIp, serverPort);
        }

        private async Task LoadServerInfo(string ip, string portString)
        {
            if (ushort.TryParse(portString, out ushort port))
            {
                var server = new SampQuery(ip, port);
                try
                {
                    var info = await server.GetServerInfoAsync();

                    Invoke(() =>
                    {
                        NameServerLabel.Text = info.HostName;
                        PlayersServerLabel.Text = $"Jogadores: {info.Players}/{info.MaxPlayers}";
                    });
                }
                catch
                {
                    Invoke(() =>
                    {
                        NameServerLabel.Text = "Servidor offline";
                        PlayersServerLabel.Text = "Jogadores: 0/0";
                    });
                }
            }
        }

        private void ConfirmButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(UserNickname))
            {
                MessageBox.Show("Por favor, insira um nickname válido.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.None;
                return;
            }

            SaveNicknameToIni(UserNickname);
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
            catch { }
        }

        private void SaveNicknameToIni(string nickname)
        {
            try
            {
                File.WriteAllText(iniPath, $"nickname={nickname}");
            }
            catch { }
        }
    }
}
