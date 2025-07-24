// ================================================
// Projeto: Emotion Launcher
// Arquivo: Launcher.cs
// Descrição: Gerencia o processo de inicialização do SA-MP e OpenMP.
// 
// Autor: Wenderson Rafael
// GitHub: https://github.com/wenderson-rafael
// Data de Criação: 24/07/2025
// Última Atualização: 24/07/2025
// 
// Licença: MIT
// ================================================

using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading.Tasks;
namespace EELauncher

{
    public partial class ELLoading : Form
    {
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect,     // x-coordinate of upper-left corner
            int nTopRect,      // y-coordinate of upper-left corner
            int nRightRect,    // x-coordinate of lower-right corner
            int nBottomRect,   // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );

        private System.Windows.Forms.Timer timer;
        public ELLoading()
        {
            InitializeComponent();

            // Aplica bordas arredondadas no Form
            this.FormBorderStyle = FormBorderStyle.None; // remove bordas padrão
            this.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 30, 30)); // bordas arredondadas

            // Inicializa o Timer (usando o namespace completo)
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 50; // a cada 50ms aumenta o progresso
            timer.Tick += Timer_Tick;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // Inicia o progresso ao carregar
            LoadingProgressBar.Minimum = 0;
            LoadingProgressBar.Maximum = 100;
            LoadingProgressBar.Value = 0;

            timer.Start();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (LoadingProgressBar.Value < LoadingProgressBar.Maximum)
            {
                LoadingProgressBar.Value += 2; // aumenta gradualmente
            }
            else
            {
                timer.Stop();
                // Abre o próximo formulário
                Form outroFormulario = new EEL(); // Substitua com o nome do seu form
                outroFormulario.Show();

                this.Hide(); // esconde o atual
            }
        }
    }
}
