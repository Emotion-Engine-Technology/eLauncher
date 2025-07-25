using System;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EELauncher
{
    public class ConfigInputBox : Form
    {
        // Import para bordas arredondadas
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(
            int nLeftRect, int nTopRect, int nRightRect, int nBottomRect,
            int nWidthEllipse, int nHeightEllipse);

        private Label lblTitle;
        private Label lblPath;
        private Button btnSelecionar;
        private Button btnBaixarGta;
        private Button btnFechar;
        private ProgressBar progressBar;
        private Label lblProgresso;

        private string iniPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini");

        public ConfigInputBox()
        {
            this.FormBorderStyle = FormBorderStyle.None;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Size = new Size(450, 250);
            this.BackColor = Color.FromArgb(28, 28, 56);
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));

            lblTitle = new Label()
            {
                Text = "Configurações",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(20, 20),
                AutoSize = true
            };

            lblPath = new Label()
            {
                Text = "Nenhum caminho selecionado",
                ForeColor = Color.Silver,
                Font = new Font("Segoe UI", 7),
                Location = new Point(20, 70),
                AutoSize = true
            };

            btnSelecionar = new Button()
            {
                Text = "Selecionar pasta do GTA SA",
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(20, 110),
                Size = new Size(200, 35)
            };
            btnSelecionar.Click += BtnSelecionar_Click;

            btnBaixarGta = new Button()
            {
                Text = "Baixar GTA SA com SA-MP",
                BackColor = Color.FromArgb(60, 60, 60),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(20, 155),
                Size = new Size(200, 35)
            };
            btnBaixarGta.Click += BtnBaixarGta_Click;

            progressBar = new ProgressBar()
            {
                Location = new Point(20, 200),
                Size = new Size(300, 20),
                Visible = false
            };

            lblProgresso = new Label()
            {
                Text = "",
                ForeColor = Color.LightGray,
                Location = new Point(20, 225),
                AutoSize = true,
                Visible = false
            };

            btnFechar = new Button()
            {
                Text = "Fechar",
                BackColor = Color.FromArgb(90, 30, 30),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Location = new Point(this.Width - 100, this.Height - 50),
                Size = new Size(90, 30),
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };
            btnFechar.Click += (s, e) => this.Close();

            if (File.Exists(iniPath))
                lblPath.Text = File.ReadAllText(iniPath);

            Controls.Add(lblTitle);
            Controls.Add(lblPath);
            Controls.Add(btnSelecionar);
            Controls.Add(btnBaixarGta);
            Controls.Add(progressBar);
            Controls.Add(lblProgresso);
            Controls.Add(btnFechar);
        }

        private void BtnSelecionar_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() == DialogResult.OK)
                {
                    lblPath.Text = folderDialog.SelectedPath;
                    File.WriteAllText(iniPath, "gta_path=" + folderDialog.SelectedPath);
                }
            }
        }

        private async void BtnBaixarGta_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
            {
                if (folderDialog.ShowDialog() != DialogResult.OK)
                    return;

                string selectedPath = folderDialog.SelectedPath;
                string zipPath = Path.Combine(selectedPath, "Rockstar.Games.rar");
                string url = "https://github.com/xWendorion/gtasa/releases/download/pp/Rockstar.Games.rar";

                btnBaixarGta.Enabled = false;
                btnBaixarGta.Text = "Baixando...";
                progressBar.Visible = true;
                lblProgresso.Visible = true;
                progressBar.Value = 0;
                lblProgresso.Text = "Progresso: 0%";

                try
                {
                    using (WebClient client = new WebClient())
                    {
                        client.DownloadProgressChanged += (s, ev) =>
                        {
                            progressBar.Value = ev.ProgressPercentage;
                            lblProgresso.Text = $"Progresso: {ev.ProgressPercentage}%";
                        };

                        await client.DownloadFileTaskAsync(new Uri(url), zipPath);
                    }

                    MessageBox.Show($"Download concluído com sucesso!\n\nLocal: {zipPath}\n\nExtraia manualmente, e, após a extração selecione a pasta do GTA SA.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    lblPath.Text = selectedPath;
                    File.WriteAllText(iniPath, "gta_path=" + selectedPath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao baixar o arquivo:\n" + ex.Message, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    btnBaixarGta.Enabled = true;
                    btnBaixarGta.Text = "Baixar GTA SA + SA-MP";
                    progressBar.Visible = false;
                    lblProgresso.Visible = false;
                }
            }
        }
    }
}
