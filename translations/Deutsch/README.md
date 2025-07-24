# Emotion Launcher

Der **Emotion Launcher (eLauncher)** ist ein moderner und dedizierter Launcher für **San Andreas Multiplayer (SA-MP)** und **Open Multiplayer (open.mp)**. Entwickelt mit Fokus auf Stabilität, Benutzerfreundlichkeit und erweiterte Funktionen, bietet der **eLauncher** ein reibungsloses Erlebnis, um sich mit Multiplayer-Servern zu verbinden, Favoriten zu verwalten, die Installation von GTA San Andreas zu konfigurieren und den Launcher automatisch und effizient zu aktualisieren.

## Sprachen

- Português: [README](../../)
- English: [README](../English/README.md)
- Español: [README](../Espanol/README.md)
- Français: [README](../Francais/README.md)
- Italiano: [README](../Italiano/README.md)
- Polski: [README](../Polski/README.md)
- Русский: [README](../Русский/README.md)
- Svenska: [README](../Svenska/README.md)
- Türkçe: [README](../Turkce/README.md)

## Inhaltsverzeichnis

- [Emotion Launcher](#emotion-launcher)
  - [Sprachen](#sprachen)
  - [Inhaltsverzeichnis](#inhaltsverzeichnis)
  - [Funktionen](#funktionen)
  - [Voraussetzungen](#voraussetzungen)
  - [Installation und Nutzung](#installation-und-nutzung)
  - [Konfiguration](#konfiguration)
  - [Verbindung zu einem Server herstellen](#verbindung-zu-einem-server-herstellen)
  - [Updatesystem](#updatesystem)
  - [Codebeispiele](#codebeispiele)
    - [Logik für die Serververbindung (vereinfacht)](#logik-für-die-serververbindung-vereinfacht)
    - [Beispiel für Konfigurationsmanager](#beispiel-für-konfigurationsmanager)
    - [Ausschnitt des Updatesystems](#ausschnitt-des-updatesystems)
  - [Bibliotheken und Abhängigkeiten von Drittanbietern](#bibliotheken-und-abhängigkeiten-von-drittanbietern)
  - [Mitwirkung](#mitwirkung)
  - [Support](#support)
  - [Lizenz](#lizenz)
  - [Danksagung](#danksagung)

## Funktionen

- **Multi-Source-Navigation:** Durchsuchen Sie lokal gespeicherte Lieblingsserver, Online-Server über die Open.MP-API und benutzerdefinierte gehostete Server.
- **Asynchrone Informationsabfrage:** Effiziente und nicht blockierende Abfrage von Serverdetails wie Name, Modus, Sprache, Spieleranzahl, Ping und Passwortstatus.
- **Favoritenverwaltung:** Hinzufügen, Entfernen und Speichern von Lieblingsservern in einer lokalen `servers.ini`-Datei.
- **Intuitiver Verbindungsdialog:** Geben Sie Ihren Nickname und das Serverpasswort (falls erforderlich) ein, mit automatischer Speicherung des Nicknames.
- **Konfiguration des GTA San Andreas-Pfads:** Wählen Sie den Installationsordner von GTA San Andreas aus oder laden Sie ein vorkonfiguriertes Paket mit GTA + SA-MP direkt über den Launcher herunter.
- **Automatische Updates:** Prüft automatisch auf neue Versionen des Launchers online und ermöglicht die Installation von Updates mit einem Klick.
- **Individuelle dunkle Benutzeroberfläche:** Modernes, konsistentes und optisch ansprechendes Design in allen Fenstern des Launchers.
- **Robuste Fehlerbehandlung:** Benutzerfreundliche Meldungen für Verbindungsprobleme, ungültige Eingaben oder Update-Fehler.

## Voraussetzungen

Um den **Emotion Launcher** auszuführen, sind folgende Komponenten erforderlich:

- [.NET Desktop Runtime 8.0 (x86)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Visual C++ Redistributable für Visual Studio 2015–2022 (x86)](https://aka.ms/vs/17/release/vc_redist.x86.exe)

> [!NOTE]
> Diese Komponenten sind zwingend erforderlich, um Ausführungsfehler zu vermeiden.

## Installation und Nutzung

1. Laden Sie die neueste Version des Launchers von der [Releases](https://github.com/xWendorion/eLauncher/releases)-Seite oder der offiziellen Website herunter: [https://elauncher.site](https://elauncher.site).
2. Entpacken Sie die heruntergeladene Datei in ein Verzeichnis Ihrer Wahl.
3. Führen Sie die Datei `EmotionLauncher.exe` aus.
4. Konfigurieren Sie den Installationspfad von GTA San Andreas über die Schaltfläche für Einstellungen.
5. Navigieren Sie durch die verfügbaren Server in den Registerkarten für Favoriten, Online-Server oder gehostete Server.
6. Doppelklicken Sie auf einen Server, um sich zu verbinden, und geben Sie bei Bedarf Ihren Nickname und das Passwort ein.

## Konfiguration

Der **Emotion Launcher** verwendet zwei Hauptkonfigurationsdateien, die sich im Verzeichnis des Launchers befinden:

- **`config.ini`**: Speichert den Installationspfad von GTA San Andreas.  
   **Beispielinhalt**:
   ```ini
   gta_path=C:\Program Files\Rockstar Games\GTA San Andreas
   ```
- **`servers.ini`**: Enthält die Liste der Lieblingsserver im Format `IP:Port`.  
   **Beispielinhalt**:
   ```ini
   127.0.0.1:7777
   play.example.com:7777
   ```

Der Pfad zu GTA San Andreas kann direkt in der Benutzeroberfläche des Launchers konfiguriert oder manuell in der Datei `config.ini` bearbeitet werden.

## Verbindung zu einem Server herstellen

1. Wählen Sie einen Server aus der Liste der Favoriten, Online-Server oder gehosteten Server aus.
2. Doppelklicken Sie auf den Server oder klicken Sie auf die Schaltfläche „Verbinden“.
3. Falls der Server ein Passwort erfordert, wird ein Eingabefenster angezeigt.
4. Geben Sie Ihren Nickname ein (wird automatisch für zukünftige Sitzungen gespeichert).
5. Klicken Sie auf „Beitreten“, um das Spiel zu starten und sich mit Hilfe von `samp-injector.dll` zu verbinden.

## Updatesystem

Der Launcher überprüft regelmäßig auf Updates über ein JSON-Manifest, das unter folgender Adresse gehostet wird:

```
https://elauncher.site/api/version/version.json
```

Wenn eine neue Version erkannt wird, zeigt der Launcher eine Benachrichtigung an, um die Aktualisierung herunterzuladen und zu installieren. Das Update-Paket wird als ZIP-Datei heruntergeladen, nach dem Schließen des Launchers entpackt, ersetzt die alten Dateien und startet die Anwendung automatisch neu.

## Codebeispiele

### Logik für die Serververbindung (vereinfacht)

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

### Beispiel für Konfigurationsmanager

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

### Ausschnitt des Updatesystems

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

## Bibliotheken und Abhängigkeiten von Drittanbietern

- **[samp-injector.dll](https://github.com/spc-samp/samp-injector/releases/tag/dll)**: Bibliothek von [SPC](https://github.com/spc-samp), für die Initialisierung und Injektion von SA-MP | open.mp.
- **[SAMPQuery](https://github.com/justmavi/sampquery)**: Bibliothek von [justmavi](https://github.com/justmavi), für die asynchrone Abfrage von Serverinformationen.
- **[Guna.UI2.WinForms](https://www.nuget.org/packages/Guna.UI2.WinForms/)**: Moderne Schnittstellensteuerungen, die im Launcher verwendet werden.

## Mitwirkung

Beiträge sind willkommen! Sie können zum **Emotion Launcher** beitragen durch:

- Melden von Fehlern und Problemen.
- Vorschläge für neue Funktionen oder Verbesserungen.
- Einreichen von Pull Requests mit Korrekturen oder neuen Funktionen.

Achten Sie darauf, den Kodierungsstil des Projekts zu befolgen und Ihre Änderungen vor dem Einreichen eines Pull Requests zu testen. Beiträge müssen mit .NET 8.0 kompatibel sein.

## Support

Bei Fragen oder Problemen kontaktieren Sie uns über:

- Eröffnen Sie eine Issue im [GitHub-Repository](https://github.com/xWendorion/eLauncher/issues).
- Kontaktformular auf der offiziellen Website: [https://elauncher.site](https://elauncher.site).

## Lizenz

Dieses Projekt, **Emotion Launcher**, ist unter der **MIT-Lizenz** lizenziert, einer weit verbreiteten und permissiven Open-Source-Lizenz. Das bedeutet, dass Sie die Freiheit haben:

- Das Software zu nutzen, zu kopieren, zu modifizieren, zusammenzuführen, zu veröffentlichen, zu verteilen, zu unterlizenzieren und/oder Kopien der Software zu verkaufen;
- Vorausgesetzt, der Copyright-Hinweis und die Lizenzbedingungen sind in allen Kopien oder wesentlichen Teilen der Software enthalten.

> [!IMPORTANT]
> Diese Software wird „wie sie ist“ bereitgestellt, ohne jegliche Garantie, weder ausdrücklich noch implizit, einschließlich, aber nicht beschränkt auf Garantien der Marktgängigkeit, Eignung für einen bestimmten Zweck und Nichtverletzung.

Für weitere rechtliche Details siehe die Datei [LICENSE](LICENSE) in diesem Repository.

## Danksagung

Vielen Dank, dass Sie den **Emotion Launcher** verwenden. Ihre Unterstützung und Beiträge helfen, das Projekt aktiv und stetig weiterzuentwickeln.