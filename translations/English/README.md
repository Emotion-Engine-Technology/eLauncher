# Emotion Launcher

![preview](https://cdn.discordapp.com/attachments/1117589516488822906/1397781579614392381/image.png?ex=6883a1ca&is=6882504a&hm=56da228f2c15718634facf0117f20a04860a0fda2f3eb2601768895e9fa370f4)

The **Emotion Launcher (eLauncher)** is a modern and dedicated launcher for **San Andreas Multiplayer (SA-MP)** and **Open Multiplayer (open.mp)**. Developed with a focus on stability, usability, and advanced features, the **eLauncher** offers a seamless experience for connecting to multiplayer servers, managing favorites, configuring the GTA San Andreas installation, and keeping the launcher updated automatically and efficiently.

## Languages

- Português: [README](../../)
- Deutsch: [README](../Deutsch/README.md)
- Español: [README](../Espanol/README.md)
- Français: [README](../Francais/README.md)
- Italiano: [README](../Italiano/README.md)
- Polski: [README](../Polski/README.md)
- Русский: [README](../Русский/README.md)
- Svenska: [README](../Svenska/README.md)
- Türkçe: [README](../Turkce/README.md)

## Table of Contents

- [Emotion Launcher](#emotion-launcher)
  - [Languages](#languages)
  - [Table of Contents](#table-of-contents)
  - [Features](#features)
  - [Requirements](#requirements)
  - [Installation and Usage](#installation-and-usage)
  - [Configuration](#configuration)
  - [How to Connect to a Server](#how-to-connect-to-a-server)
  - [Update System](#update-system)
  - [Code Examples](#code-examples)
    - [Server Connection Logic (Simplified)](#server-connection-logic-simplified)
    - [Configuration Manager Example](#configuration-manager-example)
    - [Update System Snippet](#update-system-snippet)
  - [Third-Party Libraries and Dependencies](#third-party-libraries-and-dependencies)
  - [Contribution](#contribution)
  - [Support](#support)
  - [License](#license)
  - [Thanks!](#thanks)

## Features

- **Multi-Source Navigation:** Explore locally saved favorite servers, online servers via the Open.MP API, and custom hosted servers.
- **Asynchronous Information Query:** Efficient and non-blocking retrieval of server details such as name, mode, language, player count, ping, and password status.
- **Favorites Management:** Add, remove, and persist favorite servers in a local `servers.ini` file.
- **Intuitive Connection Dialog:** Enter your nickname and server password (when required), with automatic nickname persistence.
- **GTA San Andreas Path Configuration:** Select the GTA San Andreas installation folder or download a pre-configured GTA + SA-MP package directly through the launcher.
- **Automatic Updates:** Automatically checks for new launcher versions online and allows updates to be applied with a single click.
- **Custom Dark Theme Interface:** Modern, consistent, and visually appealing design across all launcher windows.
- **Robust Error Handling:** User-friendly messages for connectivity issues, invalid inputs, or update failures.

## Requirements

To run the **Emotion Launcher**, the following components are required:

- [.NET Desktop Runtime 8.0 (x86)](https://builds.dotnet.microsoft.com/dotnet/WindowsDesktop/8.0.18/windowsdesktop-runtime-8.0.18-win-x86.exe)
- [Visual C++ Redistributable for Visual Studio 2015–2022 (x86)](https://aka.ms/vs/17/release/vc_redist.x86.exe)

> [!NOTE]
> These components are mandatory to avoid execution errors.

## Installation and Usage

1. Download the latest version of the launcher from the [Releases](https://github.com/emotionmultiplayer/eLauncher/releases) page or the official website: [https://elauncher.site](https://elauncher.site).
2. Extract the downloaded file to a directory of your choice.
3. Run the `EmotionLauncher.exe` file.
4. Configure the GTA San Andreas installation path by clicking the settings button.
5. Browse available servers in the favorites, online servers, or hosted servers tabs.
6. Double-click a server to connect, entering your nickname and password if necessary.

## Configuration

The **Emotion Launcher** uses two main configuration files located in the launcher’s directory:

- **`config.ini`**: Stores the GTA San Andreas installation path.  
   **Example content**:
   ```ini
   gta_path=C:\Program Files\Rockstar Games\GTA San Andreas
   ```
- **`servers.ini`**: Contains the list of favorite servers in the format `IP:port`.  
   **Example content**:
   ```ini
   127.0.0.1:7777
   play.example.com:7777
   ```

The GTA San Andreas path can be configured directly in the launcher’s interface or manually edited in the `config.ini` file.

## How to Connect to a Server

1. Select a server from the list of favorites, online servers, or hosted servers.
2. Double-click the server or press the “Connect” button.
3. If the server requires a password, an input window will appear.
4. Enter your nickname (automatically saved for future sessions).
5. Click “Join” to launch the game and connect using the `samp-injector.dll`.

## Update System

The launcher periodically checks for updates via a JSON manifest hosted at:

```
https://elauncher.site/api/version/version.json
```

If a new version is detected, the launcher displays a notification to download and install the update. The update package is downloaded as a ZIP file, extracted after the launcher closes, replaces the old files, and automatically restarts the application.

## Code Examples

### Server Connection Logic (Simplified)

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

## Third-Party Libraries and Dependencies

- **[samp-injector.dll](https://github.com/spc-samp/samp-injector/releases/tag/dll)**: Library from [SPC](https://github.com/spc-samp), for SA-MP | open.mp initialization and injection.
- **[SAMPQuery](https://github.com/justmavi/sampquery)**: Library from [justmavi](https://github.com/justmavi), for asynchronous server information querying.
- **[Guna.UI2.WinForms](https://www.nuget.org/packages/Guna.UI2.WinForms/)**: Modern interface controls used in the launcher.

## Contribution

Contributions are welcome! You can contribute to the **Emotion Launcher** by:

- Reporting bugs and issues.
- Suggesting new features or improvements.
- Submitting pull requests with fixes or new features.

Ensure you follow the project’s coding style and test your changes before submitting a pull request. Contributions must be compatible with .NET 8.0.

## Support

For questions or issues, contact us via:

- Opening an issue in the [GitHub repository](https://github.com/emotionmultiplayer/eLauncher/issues/new).
- Contact form on the official website: [https://elauncher.site](https://elauncher.site).

## License

This project, **Emotion Launcher**, is licensed under the **MIT License**, a widely used and permissive open-source license. This means you are free to:

- Use, copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the software;
- Provided that the copyright notice and permission notice are included in all copies or substantial portions of the software.

> [!IMPORTANT]
> This software is provided “as is,” without warranty of any kind, express or implied, including but not limited to the warranties of merchantability, fitness for a particular purpose, and noninfringement.

For further legal details, see the [LICENSE](LICENSE) file in this repository.

## Thanks!

Thank you for using the **Emotion Launcher**. Your support and contributions help keep the project active and continuously evolving.