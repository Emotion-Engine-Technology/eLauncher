# Emotion Launcher

**Emotion Launcher (eLauncher)** är en modern och dedikerad launcher för **San Andreas Multiplayer (SA-MP)** och **Open Multiplayer (open.mp)**. Utvecklad med fokus på stabilitet, användarvänlighet och avancerade funktioner, erbjuder **eLauncher** en smidig upplevelse för att ansluta till multiplayer-servrar, hantera favoriter, konfigurera installationen av GTA San Andreas och hålla launchern uppdaterad automatiskt och effektivt.

## Språk

- Português: [README](../../)
- Deutsch: [README](../Deutsch/README.md)
- English: [README](../English/README.md)
- Español: [README](../Espanol/README.md)
- Français: [README](../Francais/README.md)
- Italiano: [README](../Italiano/README.md)
- Polski: [README](../Polski/README.md)
- Русский: [README](../Русский/README.md)
- Türkçe: [README](../Turkce/README.md)

## Innehållsförteckning

- [Emotion Launcher](#emotion-launcher)
  - [Språk](#språk)
  - [Innehållsförteckning](#innehållsförteckning)
  - [Funktioner](#funktioner)
  - [Krav](#krav)
  - [Installation och användning](#installation-och-användning)
  - [Konfiguration](#konfiguration)
  - [Hur man ansluter till en server](#hur-man-ansluter-till-en-server)
  - [Uppdateringssystem](#uppdateringssystem)
  - [Kodexempel](#kodexempel)
    - [Logik för serveranslutning (förenklad)](#logik-för-serveranslutning-förenklad)
    - [Exempel på konfigurationshanterare](#exempel-på-konfigurationshanterare)
    - [Utdrag från uppdateringssystemet](#utdrag-från-uppdateringssystemet)
  - [Tredjepartsbibliotek och beroenden](#tredjepartsbibliotek-och-beroenden)
  - [Bidrag](#bidrag)
  - [Support](#support)
  - [Licens](#licens)
  - [Tack!](#tack)

## Funktioner

- **Multikällnavigering:** Utforska lokalt sparade favoritservrar, online-servrar via Open.MP API och anpassade värdservrar.
- **Asynkron informationshämtning:** Effektiv och icke-blockerande hämtning av serverdetaljer som namn, läge, språk, antal spelare, ping och lösenordsstatus.
- **Hantering av favoriter:** Lägg till, ta bort och spara favoritservrar i en lokal `servers.ini`-fil.
- **Intuitiv anslutningsdialog:** Ange ditt smeknamn och serverlösenord (vid behov), med automatisk lagring av smeknamnet.
- **Konfiguration av GTA San Andreas-sökväg:** Välj installationsmappen för GTA San Andreas eller ladda ner ett förkonfigurerat paket med GTA + SA-MP direkt via launchern.
- **Automatiska uppdateringar:** Kontrollerar automatiskt efter nya versioner av launchern online och tillåter uppdateringar med ett enda klick.
- **Anpassat mörkt temagränssnitt:** Modernt, konsekvent och visuellt tilltalande design i alla launcher-fönster.
- **Robust felhantering:** Användarvänliga meddelanden för anslutningsproblem, ogiltiga inmatningar eller uppdateringsfel.

## Krav

För att köra **Emotion Launcher** krävs följande komponenter:

- [.NET Desktop Runtime 8.0 (x86)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Visual C++ Redistributable för Visual Studio 2015–2022 (x86)](https://aka.ms/vs/17/release/vc_redist.x86.exe)

> [!NOTE]
> Dessa komponenter är obligatoriska för att undvika körningsfel.

## Installation och användning

1. Ladda ner den senaste versionen av launchern från [Releases](https://github.com/xWendorion/eLauncher/releases)-sidan eller den officiella webbplatsen: [https://elauncher.site](https://elauncher.site).
2. Extrahera den nedladdade filen till en katalog efter eget val.
3. Kör filen `EmotionLauncher.exe`.
4. Konfigurera installationssökvägen för GTA San Andreas genom att klicka på inställningsknappen.
5. Bläddra bland tillgängliga servrar i flikarna för favoriter, online-servrar eller värdservrar.
6. Dubbelklicka på en server för att ansluta, ange ditt smeknamn och lösenord om det behövs.

## Konfiguration

**Emotion Launcher** använder två huvudsakliga konfigurationsfiler som finns i launcherns katalog:

- **`config.ini`**: Lagrar installationssökvägen för GTA San Andreas.  
   **Exempel på innehåll**:
   ```ini
   gta_path=C:\Program Files\Rockstar Games\GTA San Andreas
   ```
- **`servers.ini`**: Innehåller listan över favoritservrar i formatet `IP:port`.  
   **Exempel på innehåll**:
   ```ini
   127.0.0.1:7777
   play.example.com:7777
   ```

Sökvägen till GTA San Andreas kan konfigureras direkt i launcherns gränssnitt eller redigeras manuellt i filen `config.ini`.

## Hur man ansluter till en server

1. Välj en server från listan över favoriter, online-servrar eller värdservrar.
2. Dubbelklicka på servern eller tryck på knappen "Anslut".
3. Om servern kräver ett lösenord visas ett inmatningsfönster.
4. Ange ditt smeknamn (sparas automatiskt för framtida sessioner).
5. Klicka på "Gå med" för att starta spelet och ansluta med hjälp av `samp-injector.dll`.

## Uppdateringssystem

Launchern kontrollerar periodvis efter uppdateringar via ett JSON-manifest som finns på:

```
https://elauncher.site/api/version/version.json
```

Om en ny version upptäcks visar launchern en notis för att ladda ner och installera uppdateringen. Uppdateringspaketet laddas ner som en ZIP-fil, extraheras efter att launchern stängs, ersätter de gamla filerna och startar om applikationen automatiskt.

## Kodexempel

### Logik för serveranslutning (förenklad)

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

### Exempel på konfigurationshanterare

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

### Utdrag från uppdateringssystemet

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

## Tredjepartsbibliotek och beroenden

- **[samp-injector.dll](https://github.com/spc-samp/samp-injector/releases/tag/dll)**: Bibliotek från [SPC](https://github.com/spc-samp), för initialisering och injektion av SA-MP | open.mp.
- **[SAMPQuery](https://github.com/justmavi/sampquery)**: Bibliotek från [justmavi](https://github.com/justmavi), för asynkron hämtning av serverinformation.
- **[Guna.UI2.WinForms](https://www.nuget.org/packages/Guna.UI2.WinForms/)**: Moderna gränssnittskontroller som används i launchern.

## Bidrag

Bidrag är välkomna! Du kan bidra till **Emotion Launcher** genom att:

- Rapportera buggar och problem.
- Föreslå nya funktioner eller förbättringar.
- Skicka pull requests med korrigeringar eller nya funktioner.

Se till att följa projektets kodstil och testa dina ändringar innan du skickar en pull request. Bidrag måste vara kompatibla med .NET 8.0.

## Support

Vid frågor eller problem, kontakta oss via:

- Öppna en issue i [GitHub-repositoriet](https://github.com/xWendorion/eLauncher/issues).
- Kontaktformulär på den officiella webbplatsen: [https://elauncher.site](https://elauncher.site).

## Licens

Detta projekt, **Emotion Launcher**, är licensierat under **MIT-licensen**, en allmänt använd och tillåtande open source-licens. Detta innebär att du är fri att:

- Använda, kopiera, modifiera, slå samman, publicera, distribuera, sublicensiera och/eller sälja kopior av programvaran;
- Förutsatt att upphovsrättsmeddelandet och tillståndsmeddelandet inkluderas i alla kopior eller väsentliga delar av programvaran.

> [!IMPORTANT]
> Denna programvara tillhandahålls "i befintligt skick", utan någon form av garanti, uttrycklig eller underförstådd, inklusive men inte begränsat till garantier för säljbarhet, lämplighet för ett visst ändamål och icke-intrång.

För ytterligare juridiska detaljer, se filen [LICENSE](LICENSE) i detta repositorium.

## Tack!

Tack för att du använder **Emotion Launcher**. Ditt stöd och dina bidrag hjälper till att hålla projektet aktivt och i ständig utveckling.