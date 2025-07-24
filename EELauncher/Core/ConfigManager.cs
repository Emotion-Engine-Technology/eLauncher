// ================================================
// Project: Emotion Launcher
// File: EELoading.cs
// Description: Responsible for configuring the GTA San Andreas directory.
// 
// Author: xWendorion
// GitHub: https://github.com/xWendorion
// Created: 07/01/2025
// Last Updated: 07/24/2025
// 
// License: MIT
// ================================================

﻿using System.IO;

namespace EELauncher.Core
{
    public static class ConfigManager
    {
        private static readonly string ConfigPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini");

        public static string GetGtaPath()
        {
            if (!File.Exists(ConfigPath)) return null;

            var lines = File.ReadAllLines(ConfigPath);
            foreach (var line in lines)
            {
                if (line.StartsWith("gta_path="))
                {
                    return line.Substring("gta_path=".Length);
                }
            }
            return null;
        }
    }
}
