using System;
using System.IO;
using System.Windows.Forms;
using SAMPQuery;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;
using EELauncher.CustomUI;
using EELauncher.UI;
using EELauncher.Core;
using System.Linq;
using Newtonsoft.Json;
using System.Net.Http;
using System.Runtime;
using System.Net.NetworkInformation;
using System.Net;
using IPinfo;
using System.Diagnostics;

namespace EELauncher
{
    public partial class EEL : Form
    {
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        private const int WM_NCLBUTTONDOWN = 0xA1;
        private const int HT_CAPTION = 0x2;
        private enum AbaSelecionada { Favoritos, Internet, Hosted }
        private AbaSelecionada abaAtual = AbaSelecionada.Favoritos;

        private static readonly HttpClient httpClient = new HttpClient();
        private System.Windows.Forms.Timer updateTimer;
        private string selectedIp;
        private ushort selectedPort;
        private string currentNickname = "";
        private string iniPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "settings.ini");
        public string UserNickname => nicknameTextBox.Text.Trim();

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn(

           int nLeftRect,
           int nTopRect,
           int nRightRect,
           int nBottomRect,
           int nWidthEllipse,
           int nHeightEllipse);

        public EEL()
        {
            InitializeComponent();
            Load += EEL_Load;
            this.MouseDown += Form_MouseDown;

            // Aplica bordas arredondadas no Form
            this.FormBorderStyle = FormBorderStyle.None; // remove bordas padrão
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 30, 30)); // bordas arredondada

            // Estilo de linha normal
            DataGridTabList.DefaultCellStyle.BackColor = Color.FromArgb(28, 28, 41);
            DataGridTabList.DefaultCellStyle.ForeColor = Color.White;

            // Estilo de linha alternada (igual ao normal para evitar efeito listrado)
            DataGridTabList.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(28, 28, 41);
            DataGridTabList.AlternatingRowsDefaultCellStyle.ForeColor = Color.White;
            DataGridTabList.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(13, 110, 253);
            DataGridTabList.RowsDefaultCellStyle.SelectionForeColor = Color.White;

        }

        private async void EEL_Load(object sender, EventArgs e)
        {
            // Remove colunas existentes e define 4
            DataGridTabList.ColumnCount = 4;
            DataGridTabList.Rows.Clear();
            // Carregar nickname salvo no arquivo
            LoadNicknameFromIni();
            // Torna não editável
            DataGridTabList.ReadOnly = true;

            // Configura o modo de seleção
            DataGridTabList.AllowUserToResizeColumns = false;
            DataGridTabList.AllowUserToResizeRows = false;
            DataGridTabList.AllowUserToAddRows = false;
            DataGridTabList.AllowUserToDeleteRows = false;
            DataGridTabList.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DataGridTabList.MultiSelect = false;
            this.DataGridTabList.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridTabList_CellDoubleClick);

            // Alinhamento das células e cabeçalho
            foreach (DataGridViewColumn col in DataGridTabList.Columns)
            {
                col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleLeft;
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleLeft;
            }

            // Larguras fixas
            DataGridTabList.Columns[0].Width = 300; // Nome
            DataGridTabList.Columns[1].Width = 130; // Ping
            DataGridTabList.Columns[2].Width = 190; // Modo
            DataGridTabList.Columns[3].Width = 70;  // Jogadores

            // Timer para atualização periódica
            updateTimer = new System.Windows.Forms.Timer();
            updateTimer.Interval = 1000;
            updateTimer.Tick += UpdateSelectedServer;

            // Evento clique na linha
            DataGridTabList.CellClick += DataGridTabList_CellClick;

            abaAtual = AbaSelecionada.Favoritos;
            await CarregarServidores(); // <- melhor tornar EEL_Load async, já está como async
            await SelecionarPrimeiroServidorAsync();
            //Verifica se há atualizações.
            await UpdateSystem.CheckForUpdatesAsync();
        }
        private void Form_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }
        private async void AddServerButton_Click(object sender, EventArgs e)
        {
            using (var inputDialog = new CustomInputBox("Adicionar Servidor", "Digite o IP e a porta do servidor (ex: 127.0.0.1:7777):"))
            {
                if (inputDialog.ShowDialog() == DialogResult.OK)
                {
                    string resultado = inputDialog.UserInput?.Trim();

                    if (string.IsNullOrWhiteSpace(resultado))
                    {
                        CustomMessageBox.Show("Erro", "Endereço inválido. Informe o IP ou IP:PORTA.");
                        return;
                    }

                    if (!resultado.Contains(":"))
                    {
                        resultado += ":7777";
                    }

                    string[] parts = resultado.Split(':');
                    string ip = parts[0];
                    if (!ushort.TryParse(parts[1], out ushort port))
                    {
                        CustomMessageBox.Show("Erro", "Porta inválida. Use um número entre 0 e 65535.");
                        return;
                    }

                    foreach (DataGridViewRow row in DataGridTabList.Rows)
                    {
                        if (row.Tag is ValueTuple<string, ushort> tag && tag.Item1 == ip && tag.Item2 == port)
                        {
                            CustomMessageBox.Show("Aviso", "Servidor já adicionado.");
                            return;
                        }
                    }

                    int rowIndex = DataGridTabList.Rows.Add("No Information", "0", "-", "0/0");
                    DataGridTabList.Rows[rowIndex].Tag = (ip, port);

                    await TryUpdateServerInfoAsync(rowIndex, ip, port);

                    // Salvar após adicionar
                    SalvarServidores();
                }
            }
        }
        private async void UpdateSelectedServer(object sender, EventArgs e)
        {
            if (DataGridTabList.SelectedRows.Count == 0)
                return;

            var row = DataGridTabList.SelectedRows[0];
            if (row.Tag is ValueTuple<string, ushort> tag)
            {
                string ip = tag.Item1;
                ushort port = tag.Item2;
                int index = row.Index;

                await TryUpdateServerInfoAsync(index, ip, port);
            }
        }

        private async Task TryUpdateServerInfoAsync(int rowIndex, string ip, ushort port)
        {
            try
            {
                var server = new SampQuery(ip, port);
                var info = await server.GetServerInfoAsync();

                if (abaAtual == AbaSelecionada.Internet || abaAtual == AbaSelecionada.Favoritos || abaAtual == AbaSelecionada.Hosted)
                {
                    Invoke((MethodInvoker)(() =>
                    {
                        if (rowIndex < DataGridTabList.Rows.Count)
                        {
                            DataGridTabList.Rows[rowIndex].SetValues(
                                info.HostName,
                                $"{info.ServerPing} ms",
                                info.GameMode,
                                $"{info.Players}/{info.MaxPlayers}"
                            );
                        }
                    }));

                    // Atualiza as informações no painel inferior se for o servidor selecionado
                    if (ip == selectedIp && port == selectedPort)
                        await ServerInfo(ip, port);
                }
            }
            catch
            {
                if (abaAtual == AbaSelecionada.Internet || abaAtual == AbaSelecionada.Favoritos || abaAtual == AbaSelecionada.Hosted)
                {
                    Invoke((MethodInvoker)(() =>
                    {
                        if (rowIndex < DataGridTabList.Rows.Count)
                        {
                            DataGridTabList.Rows[rowIndex].SetValues("No information", "0", "-", "0/0");
                        }
                    }));
                }
            }
        }
        private async Task ServerInfo(string ip, ushort port)
        {
            try
            {
                var server = new SampQuery(ip, port);

                // Só busca as informações do servidor (sem buscar os players)
                var info = await server.GetServerInfoAsync();

                if (!IsDisposed && !Disposing)
                {
                    Invoke(() =>
                    {
                        if (ServerNameLabel.Text != (info.HostName ?? "-"))
                            ServerNameLabel.Text = info.HostName ?? "-";

                        string ipPortText = $" {ip}:{port}";
                        if (IpAndPortLabel.Text != ipPortText)
                            IpAndPortLabel.Text = ipPortText;

                        string playersText = $"{info.Players}/{info.MaxPlayers}";
                        if (PlayersServersLabel.Text != playersText)
                            PlayersServersLabel.Text = playersText;

                        if (LangServerLabel.Text != (info.Language ?? "-"))
                            LangServerLabel.Text = info.Language ?? "-";

                        if (ModeServerLabel.Text != (info.GameMode ?? "-"))
                            ModeServerLabel.Text = info.GameMode ?? "-";
                    });
                }
            }
            catch
            {
                if (!IsDisposed && !Disposing)
                {
                    Invoke(() =>
                    {
                        ServerNameLabel.Text = "-";
                        IpAndPortLabel.Text = "-";
                        PlayersServersLabel.Text = "-";
                        LangServerLabel.Text = "-";
                        ModeServerLabel.Text = "-";
                    });
                }
            }
        }
        private async Task SelecionarPrimeiroServidorAsync()
        {
            if (DataGridTabList.Rows.Count > 0)
            {
                DataGridTabList.ClearSelection();
                DataGridTabList.Rows[0].Selected = true;

                if (DataGridTabList.Rows[0].Tag is ValueTuple<string, ushort> tag)
                {
                    selectedIp = tag.Item1;
                    selectedPort = tag.Item2;
                    await ServerInfo(selectedIp, selectedPort);
                }
            }
        }
        private async void DataGridTabList_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && DataGridTabList.Rows[e.RowIndex].Tag is ValueTuple<string, ushort> tag)
            {
                selectedIp = tag.Item1;
                selectedPort = tag.Item2;
                updateTimer.Start();

                await ServerInfo(selectedIp, selectedPort);
            }
        }

        private void DeletServerButton_Click(object sender, EventArgs e)
        {
            if (DataGridTabList.SelectedRows.Count > 0)
            {
                DataGridViewRow selectedRow = DataGridTabList.SelectedRows[0];
                DataGridTabList.Rows.Remove(selectedRow);

                // Salvar após remover
                SalvarServidores();
            }
            else
            {
                CustomMessageBox.Show("Aviso", "Nenhum servidor selecionado para remover.");
            }
        }

        private async void ConnectButton_Click(object sender, EventArgs e)
        {
            SaveNicknameToIni(UserNickname);

            if (string.IsNullOrWhiteSpace(selectedIp) || selectedPort == 0)
            {
                CustomMessageBox.Show("Aviso", "Selecione um servidor antes de conectar.");
                return;
            }

            try
            {
                var query = new SAMPQuery.SampQuery(selectedIp, selectedPort);
                var info = await query.GetServerInfoAsync();

                // Ajuste aqui conforme sua classe info
                bool requiresPassword = false;

                // Exemplo: se sua classe tiver um campo Passworded
                if (info != null)
                {

                    var prop = info.GetType().GetProperty("Passworded");
                    if (prop != null)
                    {
                        var val = prop.GetValue(info);
                        if (val is int intVal)
                            requiresPassword = (intVal == 1);
                        else if (val is bool boolVal)
                            requiresPassword = boolVal;
                    }
                }

                using (var connectBox = new ConnectInputBox(selectedIp, selectedPort.ToString(), requiresPassword))
                {
                    if (connectBox.ShowDialog() == DialogResult.OK)
                    {
                        string nickname = connectBox.UserNickname;
                        string password = connectBox.ServerPassword;

                        EELauncher.Core.SampInjector.Launch(nickname, selectedIp, selectedPort.ToString(), password);
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Erro", $"Falha ao consultar o servidor: {ex.Message}");
            }
        }

        private void ConfigButton_Click(object sender, EventArgs e)
        {
            ConfigInputBox configForm = new ConfigInputBox();
            configForm.ShowDialog();
        }

        // Salva os servidores atuais no arquivo servers.ini
        private void SalvarServidores()
        {
            try
            {
                string caminhoIni = Path.Combine(Application.StartupPath, "servers.ini");
                using (StreamWriter writer = new StreamWriter(caminhoIni, false))
                {
                    foreach (DataGridViewRow row in DataGridTabList.Rows)
                    {
                        if (row.Tag is ValueTuple<string, ushort> tag)
                        {
                            string ip = tag.Item1;
                            ushort port = tag.Item2;
                            writer.WriteLine($"{ip}:{port}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Erro", "Erro ao salvar os servidores: " + ex.Message);
            }
        }

        // Carrega servidores salvos do arquivo servers.ini
        private async Task CarregarServidores()
        {
            try
            {
                // ✅ Limpa a tabela ao carregar favoritos
                DataGridTabList.Rows.Clear();

                string caminhoIni = Path.Combine(Application.StartupPath, "servers.ini");
                if (!File.Exists(caminhoIni)) return;

                var linhas = File.ReadAllLines(caminhoIni);

                foreach (string linha in linhas)
                {
                    if (string.IsNullOrWhiteSpace(linha)) continue;

                    string[] parts = linha.Split(':');
                    if (parts.Length != 2) continue;

                    string ip = parts[0];
                    if (!ushort.TryParse(parts[1], out ushort port)) continue;

                    // Evita duplicados
                    bool existe = false;
                    foreach (DataGridViewRow row in DataGridTabList.Rows)
                    {
                        if (row.Tag is ValueTuple<string, ushort> tag && tag.Item1 == ip && tag.Item2 == port)
                        {
                            existe = true;
                            break;
                        }
                    }
                    if (existe) continue;

                    int rowIndex = DataGridTabList.Rows.Add("No Information", "0", "-", "0/0");
                    DataGridTabList.Rows[rowIndex].Tag = (ip, port);

                    await TryUpdateServerInfoAsync(rowIndex, ip, port);
                }
            }
            catch (Exception ex)
            {
                CustomMessageBox.Show("Erro", "Erro ao carregar servidores: " + ex.Message);
            }
        }
        private async void FavServersButton_Click(object sender, EventArgs e)
        {
            abaAtual = AbaSelecionada.Favoritos;
            updateTimer.Stop();
            DataGridTabList.Rows.Clear();
            await CarregarServidores();
            await SelecionarPrimeiroServidorAsync();
        }

        private async void InternetServersButton_Click(object sender, EventArgs e)
        {
            abaAtual = AbaSelecionada.Internet;
            await SelecionarPrimeiroServidorAsync();
            updateTimer.Stop();
            DataGridTabList.Rows.Clear();

            try
            {
                string url = "https://api.open.mp/servers";
                string json = await httpClient.GetStringAsync(url);

                var servers = JsonConvert.DeserializeObject<List<SampServerEntry>>(json);
                var limitedServers = servers.Take(200); // ✅ Limite aplicado

                foreach (var srv in limitedServers)
                {
                    if (abaAtual != AbaSelecionada.Internet) break; // ✅ Evita carregar se o usuário já trocou de aba

                    string ip = srv.ip;
                    ushort port = srv.port;

                    if (ip.Contains(":"))
                    {
                        var parts = ip.Split(':');
                        ip = parts[0];
                        ushort.TryParse(parts[1], out port);
                    }

                    int rowIndex = DataGridTabList.Rows.Add("No Information", "0", "-", "0/0");
                    DataGridTabList.Rows[rowIndex].Tag = (ip, port);

                    await TryUpdateServerInfoAsync(rowIndex, ip, port);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar servidores: " + ex.Message);
            }
        }
        private async void HostedButton_Click(object sender, EventArgs e)
        {
            abaAtual = AbaSelecionada.Hosted;
            await SelecionarPrimeiroServidorAsync();
            updateTimer.Stop();
            DataGridTabList.Rows.Clear();

            try
            {
                string url = "https://elauncher.site/api/servers/partners.json";
                string json = await httpClient.GetStringAsync(url);

                var servers = JsonConvert.DeserializeObject<List<SampServerEntry>>(json);

                foreach (var srv in servers)
                {
                    if (abaAtual != AbaSelecionada.Hosted) break; // <- Correção aqui

                    string ip = srv.ip;
                    ushort port = srv.port;

                    int rowIndex = DataGridTabList.Rows.Add("No Information", "0", "-", "0/0");
                    DataGridTabList.Rows[rowIndex].Tag = (ip, port);

                    await TryUpdateServerInfoAsync(rowIndex, ip, port);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar servidores personalizados: " + ex.Message);
            }
        }
        public class SampServerEntry
        {
            public string ip { get; set; }

            public ushort port { get; set; }
        }


        private async void dataGridTabList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return; // Linha inválida

            var row = DataGridTabList.Rows[e.RowIndex];

            if (row.Tag is ValueTuple<string, ushort> tag)
            {
                string ip = tag.Item1;
                ushort port = tag.Item2;

                if (string.IsNullOrWhiteSpace(ip) || port == 0)
                {
                    CustomMessageBox.Show("Erro", "IP ou porta inválidos.");
                    return;
                }

                SaveNicknameToIni(UserNickname);

                try
                {
                    var query = new SAMPQuery.SampQuery(ip, port);
                    var info = await query.GetServerInfoAsync();

                    bool requiresPassword = false;

                    if (info != null)
                    {
                        var prop = info.GetType().GetProperty("Passworded");
                        if (prop != null)
                        {
                            var val = prop.GetValue(info);
                            if (val is int intVal)
                                requiresPassword = (intVal == 1);
                            else if (val is bool boolVal)
                                requiresPassword = boolVal;
                        }
                    }

                    using (var connectBox = new ConnectInputBox(ip, port.ToString(), requiresPassword))
                    {
                        if (connectBox.ShowDialog() == DialogResult.OK)
                        {
                            string nickname = connectBox.UserNickname;
                            string password = connectBox.ServerPassword;

                            EELauncher.Core.SampInjector.Launch(nickname, ip, port.ToString(), password);
                        }
                    }
                }
                catch (Exception ex)
                {
                    CustomMessageBox.Show("Erro", $"Falha ao consultar o servidor: {ex.Message}");
                }
            }
            else
            {
                CustomMessageBox.Show("Erro", "Servidor não selecionado corretamente.");
            }
        }

        private void CloseButton_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void CreditsButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
              "Emotion Launcher - Release 0.2.0 uB\n\n" +
              "Desenvolvido por xWendorion (github.com/xWendorion)\n" +
              "Agradecimento ao w0nht que contribuiu para o desenvolvimento do Launcher\n" +
              "Agradecimento especial ao Calasans pela contribuição com o 'samp-injector'.\n\n" +
              "Este launcher foi criado com dedicação à comunidade SA-MP brasileira, que mantém viva a paixão por San Andreas Multiplayer há tantos anos.\n\n" +
              "Obrigado a todos que apoiam, testam e colaboram para a melhoria deste projeto.\n" +
              "Que possamos continuar evoluindo juntos!\n\n" +
              "Informações Técnicas:\n" +
              "• Usando a versão do .NET 8.0 Desktop Runtime\n" +
              "• Plataforma: Windows x64\n" +
              "• Build: Estável (Stable Build)",
              "Sobre o Launcher",
              MessageBoxButtons.OK,
              MessageBoxIcon.Information
            );


        }

        private void label12_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(new ProcessStartInfo
            {
                FileName = "https://elauncher.site",
                UseShellExecute = true
            });
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

        private void label7_Click(object sender, EventArgs e)
        {

        }
    }
}
