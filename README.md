
# Emotion Launcher - Launcher for SA-MP and OpenMP

---

## Overview

**Emotion Launcher (eLauncher)** is a dedicated, modern launcher designed for **San Andreas Multiplayer (SA-MP)** and **Open Multiplayer (OpenMP)**. It aims to provide a stable, user-friendly, and feature-rich experience for connecting to multiplayer servers, managing favorites, configuring your GTA San Andreas installation, and keeping the launcher updated seamlessly.

This repository contains the source code, binaries, and all resources related to the launcher.

---

## Table of Contents

- [Features](#features)  
- [Requirements](#requirements)  
- [Installation and Usage](#installation-and-usage)  
- [Configuration](#configuration)  
- [How to Connect to a Server](#how-to-connect-to-a-server)  
- [Update System](#update-system)  
- [Code Examples](#code-examples)  
- [Third-Party Libraries and Dependencies](#third-party-libraries-and-dependencies)  
- [Contributing](#contributing)  
- [Support](#support)  
- [License](#license)  

---

## Features

- **Multi-Source Server Browsing:** Browse favorite servers saved locally, internet servers via Open.MP API, and hosted custom servers.  
- **Asynchronous Server Info Querying:** Efficient and non-blocking retrieval of server details like name, mode, language, players, ping, and password status.  
- **Favorites Management:** Easily add, remove, and persist favorite servers in a local `servers.ini` file.  
- **User-Centric Connection Dialog:** Enter your nickname and server password if required, with automatic nickname persistence.  
- **GTA San Andreas Path Configuration:** Select your GTA SA installation folder or download a pre-packaged GTA + SA-MP archive from within the launcher.  
- **Automated Update Mechanism:** Checks for newer launcher versions online and prompts the user to download and apply updates automatically.  
- **Custom Dark-Themed UI:** Consistent, modern, and clean interface across the entire launcher and its dialogs.  
- **Robust Error Handling:** User-friendly messages for connectivity issues, invalid inputs, and update failures.  

---

## Requirements

Before running Emotion Launcher, ensure the following components are installed:

- [.NET Desktop Runtime 8.0 (x64)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)  
- [Visual C++ Redistributable for Visual Studio 2015–2022 (x86)](https://aka.ms/vs/17/release/vc_redist.x86.exe)  

**Note:** These components are mandatory for the launcher to function correctly and to avoid runtime errors.

---

## Installation and Usage

1. Download the latest release from the [Releases](https://github.com/xWendorion/eLauncher/releases) page or from the official website: [https://elauncher.site](https://elauncher.site).  
2. Extract the downloaded archive to a preferred directory.  
3. Run `EmotionLauncher.exe`.  
4. Configure your GTA San Andreas installation path by clicking the settings/configuration button.  
5. Browse available servers in your favorites, online listings, or hosted servers tabs.  
6. Double-click a server to connect, entering your nickname and password if required.  

---

## Configuration

The launcher uses two primary configuration files located in the launcher directory:

- **`config.ini`** — Stores the GTA San Andreas installation path.  
  Example content:
  ```ini
  gta_path=C:\Program Files\Rockstar Games\GTA San Andreas
```

* **`servers.ini`** — Stores your list of favorite servers as IP\:port pairs.
  Example content:

  ```
  127.0.0.1:7777
  play.example.com:7777
  ``

You can set or change the GTA path inside the launcher UI or manually edit the `config.ini`.

---

## How to Connect to a Server

* Select a server from any server list (Favorites, Internet, Hosted).
* Double-click the desired server entry or press the "Connect" button.
* If the server requires a password, a password input will be displayed.
* Enter your nickname (saved automatically for future sessions).
* Click "Join" to launch the game and connect using the custom `samp-injector.dll`.

---

## Update System

The launcher periodically checks for updates using a remote JSON manifest located at:

```
https://elauncher.site/api/version/version.json
```

If a newer version is found, the launcher prompts the user to download and install the update. The update package is downloaded as a zip file and extracted after the launcher exits, replacing the older files and restarting the application automatically.

---

## Code Examples

### Server Connection Logic (simplified)

```csharp
private async void ConnectButton_Click(object sender, EventArgs e)
{
    SaveNicknameToIni(UserNickname);

    if (string.IsNullOrWhiteSpace(selectedIp) || selectedPort == 0)
    {
        CustomMessageBox.Show("Warning", "Please select a server before connecting.");
        return;
    }

    try
    {
        var query = new SAMPQuery.SampQuery(selectedIp, selectedPort);
        var info = await query.GetServerInfoAsync();

        bool requiresPassword = false;
        if (info != null)
        {
            var prop = info.GetType().GetProperty("Passworded");
            if (prop != null)
            {
                var val = prop.GetValue(info);
                requiresPassword = val is int intVal ? intVal == 1 : val is bool boolVal && boolVal;
            }
        }

        using var connectBox = new ConnectInputBox(selectedIp, selectedPort.ToString(), requiresPassword);
        if (connectBox.ShowDialog() == DialogResult.OK)
        {
            string nickname = connectBox.UserNickname;
            string password = connectBox.ServerPassword;

            EELauncher.Core.SampInjector.Launch(nickname, selectedIp, selectedPort.ToString(), password);
        }
    }
    catch (Exception ex)
    {
        CustomMessageBox.Show("Error", $"Failed to query server info: {ex.Message}");
    }
}
```

### Configuration Manager Example

```csharp
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
```

### Update System Snippet

```csharp
public static async Task<bool> CheckForUpdatesAsync()
{
    try
    {
        using var client = new HttpClient();
        var json = await client.GetStringAsync(versionUrl);
        var serverData = JsonSerializer.Deserialize<VersionData>(json);

        if (serverData != null && new Version(serverData.version) > new Version(LocalVersion))
        {
            var result = MessageBox.Show($"New version available: {serverData.version}\nDo you want to update now?",
                "Emotion Launcher - Update Available", MessageBoxButtons.YesNo, MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                await DownloadAndUpdateAsync(serverData.downloadUrl, serverData.version);
                return true;
            }
        }
    }
    catch (Exception ex)
    {
        MessageBox.Show("Error checking updates: " + ex.Message);
    }

    return false;
}
```
---

## Requirements

To run **eLauncher** properly, you must have the following installed:

- [.NET 8.0 Runtime (x86)](https://dotnet.microsoft.com/pt-br/download/dotnet/thank-you/runtime-desktop-8.0.18-windows-x86-installer?cid=getdotnetcore)
- [Visual C++ Redistributable 2015–2022 (x86)](https://aka.ms/vs/17/release/vc_redist.x86.exe)

> **These components are required.**  
> Make sure to install both before running the launcher to avoid errors.

---
---

## Third-Party Libraries and Dependencies

* [samp-injector.dll](https://github.com/spc-samp/samp-injector) by SPC/Calasans — For launching and injecting SA-MP.
* [SAMPQuery](https://github.com/justmavi/sampquery) by justmavi — For querying server information asynchronously.
* [Guna.UI2.WinForms](https://www.nuget.org/packages/Guna.UI2.WinForms/) — Modern UI controls used in the launcher.

---

## Contributing

Contributions are welcome! You can help improve **Emotion Launcher** by:

* Reporting bugs and issues.
* Suggesting new features or enhancements.
* Submitting pull requests with bug fixes or new functionality.

Please make sure to follow the code style and test your changes before submitting a PR. Contributions should target the latest .NET 8.0 runtime.

---

## Support

If you encounter any issues or have questions, please:

* Open an issue on the [GitHub repository](https://github.com/xWendorion/eLauncher/issues).
* Contact the maintainers via the official website contact form.

---

## License

Emotion Launcher is open source under the **MIT License**. See the [LICENSE](LICENSE) file for details.

---

Thank you for using **Emotion Launcher**. Your support and contributions help us keep the project alive and evolving.

