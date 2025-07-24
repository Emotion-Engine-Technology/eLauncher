using System.IO;

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
