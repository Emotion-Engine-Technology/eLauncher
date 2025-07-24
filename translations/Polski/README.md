# Emotion Launcher

**Emotion Launcher (eLauncher)** to nowoczesny i dedykowany launcher dla **San Andreas Multiplayer (SA-MP)** oraz **Open Multiplayer (open.mp)**. Zaprojektowany z naciskiem na stabilność, użyteczność i zaawansowane funkcje, **eLauncher** oferuje płynne doświadczenie w łączeniu się z serwerami wieloosobowymi, zarządzaniu ulubionymi, konfigurowaniu instalacji GTA San Andreas oraz automatycznym i efektywnym aktualizowaniu launchera.

## Języki

- Português: [README](../../)
- Deutsch: [README](../Deutsch/README.md)
- English: [README](../English/README.md)
- Español: [README](../Espanol/README.md)
- Français: [README](../Francais/README.md)
- Italiano: [README](../Italiano/README.md)
- Русский: [README](../Русский/README.md)
- Svenska: [README](../Svenska/README.md)
- Türkçe: [README](../Turkce/README.md)

## Spis treści

- [Emotion Launcher](#emotion-launcher)
  - [Języki](#języki)
  - [Spis treści](#spis-treści)
  - [Funkcje](#funkcje)
  - [Wymagania](#wymagania)
  - [Instalacja i użytkowanie](#instalacja-i-użytkowanie)
  - [Konfiguracja](#konfiguracja)
  - [Jak połączyć się z serwerem](#jak-połączyć-się-z-serwerem)
  - [System aktualizacji](#system-aktualizacji)
  - [Przykłady kodu](#przykłady-kodu)
    - [Logika połączenia z serwerami (uproszczona)](#logika-połączenia-z-serwerami-uproszczona)
    - [Przykład menedżera konfiguracji](#przykład-menedżera-konfiguracji)
    - [Fragment systemu aktualizacji](#fragment-systemu-aktualizacji)
  - [Biblioteki i zależności zewnętrzne](#biblioteki-i-zależności-zewnętrzne)
  - [Wkład](#wkład)
  - [Wsparcie](#wsparcie)
  - [Licencja](#licencja)
  - [Podziękowania](#podziękowania)

## Funkcje

- **Nawigacja wieloźródłowa:** Przeglądaj zapisane lokalnie ulubione serwery, serwery online za pośrednictwem API Open.MP oraz niestandardowe serwery hostowane.
- **Asynchroniczne zapytania o informacje:** Efektywne i nieblokujące pobieranie szczegółów serwera, takich jak nazwa, tryb, język, liczba graczy, ping i status hasła.
- **Zarządzanie ulubionymi:** Dodawaj, usuwaj i zapisuj ulubione serwery w lokalnym pliku `servers.ini`.
- **Intuicyjny dialog połączenia:** Wprowadź swój pseudonim i hasło serwera (jeśli wymagane), z automatycznym zapisywaniem pseudonimu.
- **Konfiguracja ścieżki GTA San Andreas:** Wybierz folder instalacji GTA San Andreas lub pobierz wstępnie skonfigurowany pakiet GTA + SA-MP bezpośrednio przez launcher.
- **Automatyczne aktualizacje:** Automatycznie sprawdza nowe wersje launchera online i umożliwia zastosowanie aktualizacji jednym kliknięciem.
- **Spersonalizowany ciemny motyw interfejsu:** Nowoczesny, spójny i atrakcyjny wizualnie projekt we wszystkich oknach launchera.
- **Solidna obsługa błędów:** Przyjazne dla użytkownika komunikaty dla problemów z łącznością, nieprawidłowych danych wejściowych lub niepowodzeń aktualizacji.

## Wymagania

Aby uruchomić **Emotion Launcher**, wymagane są następujące komponenty:

- [.NET Desktop Runtime 8.0 (x86)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Visual C++ Redistributable dla Visual Studio 2015–2022 (x86)](https://aka.ms/vs/17/release/vc_redist.x86.exe)

> [!NOTE]
> Te komponenty są obowiązkowe, aby uniknąć błędów wykonania.

## Instalacja i użytkowanie

1. Pobierz najnowszą wersję launchera ze strony [Releases](https://github.com/xWendorion/eLauncher/releases) lub oficjalnej strony: [https://elauncher.site](https://elauncher.site).
2. Wypakuj pobrany plik do wybranego katalogu.
3. Uruchom plik `EmotionLauncher.exe`.
4. Skonfiguruj ścieżkę instalacji GTA San Andreas, klikając przycisk ustawień.
5. Przeglądaj dostępne serwery w zakładkach ulubionych, serwerów online lub hostowanych.
6. Kliknij dwukrotnie serwer, aby się połączyć, wprowadzając pseudonim i hasło, jeśli jest to wymagane.

## Konfiguracja

**Emotion Launcher** używa dwóch głównych plików konfiguracyjnych, zlokalizowanych w katalogu launchera:

- **`config.ini`**: Przechowuje ścieżkę instalacji GTA San Andreas.  
   **Przykładowa zawartość**:
   ```ini
   gta_path=C:\Program Files\Rockstar Games\GTA San Andreas
   ```
- **`servers.ini`**: Zawiera listę ulubionych serwerów w formacie `IP:port`.  
   **Przykładowa zawartość**:
   ```ini
   127.0.0.1:7777
   play.example.com:7777
   ```

Ścieżkę GTA San Andreas można skonfigurować bezpośrednio w interfejsie launchera lub ręcznie edytując plik `config.ini`.

## Jak połączyć się z serwerem

1. Wybierz serwer z listy ulubionych, serwerów online lub hostowanych.
2. Kliknij dwukrotnie serwer lub naciśnij przycisk „Połącz”.
3. Jeśli serwer wymaga hasła, pojawi się okno wprowadzania.
4. Wprowadź swój pseudonim (zapisywany automatycznie dla przyszłych sesji).
5. Kliknij „Dołącz”, aby uruchomić grę i połączyć się za pomocą `samp-injector.dll`.

## System aktualizacji

Launcher okresowo sprawdza aktualizacje za pośrednictwem manifestu JSON hostowanego pod adresem:

```
https://elauncher.site/api/version/version.json
```

Jeśli zostanie wykryta nowa wersja, launcher wyświetla powiadomienie o możliwości pobrania i instalacji aktualizacji. Pakiet aktualizacji jest pobierany jako plik ZIP, wypakowywany po zamknięciu launchera, zastępuje stare pliki i automatycznie uruchamia ponownie aplikację.

## Przykłady kodu

### Logika połączenia z serwerami (uproszczona)

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

### Przykład menedżera konfiguracji

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

### Fragment systemu aktualizacji

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

## Biblioteki i zależności zewnętrzne

- **[samp-injector.dll](https://github.com/spc-samp/samp-injector/releases/tag/dll)**: Biblioteka od [SPC](https://github.com/spc-samp), dla inicjalizacji i wstrzykiwania SA-MP | open.mp.
- **[SAMPQuery](https://github.com/justmavi/sampquery)**: Biblioteka od [justmavi](https://github.com/justmavi), dla asynchronicznego zapytania o informacje o serwerach.
- **[Guna.UI2.WinForms](https://www.nuget.org/packages/Guna.UI2.WinForms/)**: Nowoczesne kontrolki interfejsu używane w launcherze.

## Wkład

Wkład jest mile widziany! Możesz przyczynić się do **Emotion Launcher** poprzez:

- Zgłaszanie błędów i problemów.
- Sugerowanie nowych funkcji lub ulepszeń.
- Wysłanie pull requestów z poprawkami lub nowymi funkcjami.

Upewnij się, że przestrzegasz stylu kodowania projektu i testujesz swoje zmiany przed przesłaniem pull requestu. Wkład musi być kompatybilny z .NET 8.0.

## Wsparcie

W przypadku pytań lub problemów skontaktuj się z nami poprzez:

- Otwarcie zgłoszenia w [repozytorium GitHub](https://github.com/xWendorion/eLauncher/issues).
- Formularz kontaktowy na oficjalnej stronie: [https://elauncher.site](https://elauncher.site).

## Licencja

Ten projekt, **Emotion Launcher**, jest licencjonowany na podstawie **Licencji MIT**, szeroko stosowanej i permisywnej licencji open-source. Oznacza to, że masz swobodę:

- Używania, kopiowania, modyfikowania, łączenia, publikowania, dystrybucji, sublicencjonowania i/lub sprzedaży kopii oprogramowania;
- Pod warunkiem, że powiadomienie o prawach autorskich i zezwolenie na licencję są zawarte we wszystkich kopiach lub istotnych częściach oprogramowania.

> [!IMPORTANT]
> To oprogramowanie jest dostarczane „takie, jakie jest”, bez jakiejkolwiek gwarancji, wyraźnej lub dorozumianej, w tym między innymi gwarancji przydatności handlowej, przydatności do określonego celu oraz nienaruszania praw.

Aby uzyskać więcej szczegółów prawnych, zobacz plik [LICENSE](LICENSE) w tym repozytorium.

## Podziękowania

Dziękujemy za korzystanie z **Emotion Launcher**. Twoje wsparcie i wkład pomagają utrzymać projekt aktywnym i w ciągłym rozwoju.