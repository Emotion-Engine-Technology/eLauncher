// ================================================
// Project: Emotion Launcher
// File: UpdateSystem.cs
// Description: System responsible for checking and updating the launcher when a newer version file is available.
// 
// Author: xWendorion
// GitHub: https://github.com/xWendorion
// Created: 07/01/2025
// Last Updated: 07/24/2025
// 
// License: MIT
// ================================================
﻿using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;

namespace EELauncher.Core
{
    internal class UpdateSystem
    {
        private static readonly string versionUrl = "https://elauncher.site/api/version/version.json";
        private static readonly string localVersionFile = "version.ini";
        private static readonly string updateZipFile = "update.zip";

        public static string LocalVersion => File.Exists(localVersionFile) ? File.ReadAllText(localVersionFile) : "0.0.0";

        public class VersionData
        {
            public string version { get; set; }
            public string downloadUrl { get; set; }
        }

        public static async Task<bool> CheckForUpdatesAsync()
        {
            try
            {
                using var client = new HttpClient();
                var json = await client.GetStringAsync(versionUrl);
                var serverData = JsonSerializer.Deserialize<VersionData>(json);

                if (serverData != null && new Version(serverData.version) > new Version(LocalVersion))
                {
                    var result = MessageBox.Show($"Nova versão disponível: {serverData.version}\nDeseja atualizar agora?",
                        "Emotion Launcher - Atualização disponível", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

                    if (result == DialogResult.Yes)
                    {
                        await DownloadAndUpdateAsync(serverData.downloadUrl, serverData.version);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao verificar atualizações: " + ex.Message);
            }

            return false;
        }

        private static async Task DownloadAndUpdateAsync(string url, string newVersion)
        {
            try
            {
                using var client = new HttpClient();
                var data = await client.GetByteArrayAsync(url);
                File.WriteAllBytes(updateZipFile, data);

                string exePath = Application.ExecutablePath;
                string batPath = Path.Combine(Path.GetTempPath(), "update_launcher.bat");
                string extractPath = AppDomain.CurrentDomain.BaseDirectory;

                File.WriteAllText(batPath, $@"
@echo off
timeout /t 2 > nul
:loop
tasklist | find /i ""{Path.GetFileName(exePath)}"" > nul
if not errorlevel 1 (
    timeout /t 1 > nul
    goto loop
)
powershell -Command ""Expand-Archive -Path '{updateZipFile}' -DestinationPath '{extractPath}' -Force""
del /f /q ""{updateZipFile}""
echo {newVersion} > ""{localVersionFile}""
start """" ""{exePath}""
del ""%~f0""
");

                Process.Start(new ProcessStartInfo
                {
                    FileName = batPath,
                    UseShellExecute = true,
                    CreateNoWindow = true,
                    WindowStyle = ProcessWindowStyle.Hidden
                });

                Environment.Exit(0);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao atualizar: " + ex.Message);
            }
        }
    }
}
