// ================================================
// Project: Emotion Launcher
// File: EELoading.cs
// Description: Responsible for the injection and startup process of SA-MP or OMP.
// 
// Author: xWendorion
// GitHub: https://github.com/xWendorion
// Created: 07/01/2025
// Last Updated: 07/24/2025
// 
// License: MIT
// ================================================

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace EELauncher.Core
{
    public static class SampInjector
    {
        [DllImport("samp-injector.dll", CharSet = CharSet.Unicode, CallingConvention = CallingConvention.StdCall, EntryPoint = "Launch_Game")]
        private static extern int Launch_Game(string mode, string folder, string nickname, string ip, string port, string password);

        public static void Launch(string nickname, string ip, string port, string password)
        {
            string gtaPath = ConfigManager.GetGtaPath();
            if (string.IsNullOrWhiteSpace(gtaPath))
            {
                MessageBox.Show("Caminho do GTA não configurado.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int result = Launch_Game("samp", gtaPath, nickname, ip, port, password);
            if (result != 0)
            {
                MessageBox.Show($"Erro ao iniciar o jogo. Código: {result}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

