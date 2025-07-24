# Emotion Launcher

L'**Emotion Launcher (eLauncher)** è un launcher moderno e dedicato per **San Andreas Multiplayer (SA-MP)** e **Open Multiplayer (open.mp)**. Sviluppato con un'attenzione particolare alla stabilità, all'usabilità e alle funzionalità avanzate, l'**eLauncher** offre un'esperienza fluida per connettersi ai server multigiocatore, gestire i preferiti, configurare l'installazione di GTA San Andreas e mantenere il launcher aggiornato in modo automatico ed efficiente.

## Lingue

- Português: [README](../../)
- Deutsch: [README](../Deutsch/README.md)
- English: [README](../English/README.md)
- Español: [README](../Espanol/README.md)
- Français: [README](../Francais/README.md)
- Polski: [README](../Polski/README.md)
- Русский: [README](../Русский/README.md)
- Svenska: [README](../Svenska/README.md)
- Türkçe: [README](../Turkce/README.md)

## Indice

- [Emotion Launcher](#emotion-launcher)
  - [Lingue](#lingue)
  - [Indice](#indice)
  - [Funzionalità](#funzionalità)
  - [Requisiti](#requisiti)
  - [Installazione e utilizzo](#installazione-e-utilizzo)
  - [Configurazione](#configurazione)
  - [Come connettersi a un server](#come-connettersi-a-un-server)
  - [Sistema di aggiornamento](#sistema-di-aggiornamento)
  - [Esempi di codice](#esempi-di-codice)
    - [Logica di connessione ai server (semplificata)](#logica-di-connessione-ai-server-semplificata)
    - [Esempio di gestore di configurazione](#esempio-di-gestore-di-configurazione)
    - [Frammento del sistema di aggiornamento](#frammento-del-sistema-di-aggiornamento)
  - [Librerie e dipendenze di terze parti](#librerie-e-dipendenze-di-terze-parti)
  - [Contribuzione](#contribuzione)
  - [Supporto](#supporto)
  - [Licenza](#licenza)
  - [Ringraziamenti](#ringraziamenti)

## Funzionalità

- **Navigazione multi-sorgente:** Esplora i server preferiti salvati localmente, i server online tramite l'API di Open.MP e i server personalizzati ospitati.
- **Interrogazione asincrona delle informazioni:** Recupero efficiente e non bloccante dei dettagli del server come nome, modalità, lingua, numero di giocatori, ping e stato della password.
- **Gestione dei preferiti:** Aggiungi, rimuovi e salva i server preferiti in un file locale `servers.ini`.
- **Dialogo di connessione intuitivo:** Inserisci il tuo nickname e la password del server (se richiesta), con persistenza automatica del nickname.
- **Configurazione del percorso di GTA San Andreas:** Seleziona la cartella di installazione di GTA San Andreas o scarica un pacchetto preconfigurato con GTA + SA-MP direttamente tramite il launcher.
- **Aggiornamenti automatici:** Verifica automaticamente la presenza di nuove versioni del launcher online e consente di applicare gli aggiornamenti con un solo clic.
- **Interfaccia con tema scuro personalizzato:** Design moderno, coerente e visivamente accattivante in tutte le finestre del launcher.
- **Gestione robusta degli errori:** Messaggi user-friendly per problemi di connettività, input non validi o errori di aggiornamento.

## Requisiti

Per eseguire l'**Emotion Launcher**, sono necessari i seguenti componenti:

- [.NET Desktop Runtime 8.0 (x86)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Visual C++ Redistributable per Visual Studio 2015–2022 (x86)](https://aka.ms/vs/17/release/vc_redist.x86.exe)

> [!NOTE]
> Questi componenti sono obbligatori per evitare errori di esecuzione.

## Installazione e utilizzo

1. Scarica l'ultima versione del launcher dalla pagina [Releases](https://github.com/xWendorion/eLauncher/releases) o dal sito ufficiale: [https://elauncher.site](https://elauncher.site).
2. Estrai il file scaricato in una directory di tua scelta.
3. Esegui il file `EmotionLauncher.exe`.
4. Configura il percorso di installazione di GTA San Andreas cliccando sul pulsante delle impostazioni.
5. Naviga tra i server disponibili nelle schede dei preferiti, dei server online o dei server ospitati.
6. Fai doppio clic su un server per connetterti, inserendo il tuo nickname e la password, se necessario.

## Configurazione

L'**Emotion Launcher** utilizza due file di configurazione principali, situati nella directory del launcher:

- **`config.ini`**: Memorizza il percorso di installazione di GTA San Andreas.  
   **Esempio di contenuto**:
   ```ini
   gta_path=C:\Program Files\Rockstar Games\GTA San Andreas
   ```
- **`servers.ini`**: Contiene l'elenco dei server preferiti nel formato `IP:porta`.  
   **Esempio di contenuto**:
   ```ini
   127.0.0.1:7777
   play.example.com:7777
   ```

Il percorso di GTA San Andreas può essere configurato direttamente nell'interfaccia del launcher o modificato manualmente nel file `config.ini`.

## Come connettersi a un server

1. Seleziona un server dall'elenco dei preferiti, dei server online o dei server ospitati.
2. Fai doppio clic sul server o premi il pulsante "Connetti".
3. Se il server richiede una password, apparirà una finestra di inserimento.
4. Inserisci il tuo nickname (salvato automaticamente per le sessioni future).
5. Clicca su "Entra" per avviare il gioco e connetterti utilizzando `samp-injector.dll`.

## Sistema di aggiornamento

Il launcher verifica periodicamente la presenza di aggiornamenti tramite un manifesto JSON ospitato all'indirizzo:

```
https://elauncher.site/api/version/version.json
```

Se viene rilevata una nuova versione, il launcher mostra una notifica per scaricare e installare l'aggiornamento. Il pacchetto di aggiornamento viene scaricato come file ZIP, estratto dopo la chiusura del launcher, sostituisce i vecchi file e riavvia automaticamente l'applicazione.

## Esempi di codice

### Logica di connessione ai server (semplificata)

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

### Esempio di gestore di configurazione

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

### Frammento del sistema di aggiornamento

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

## Librerie e dipendenze di terze parti

- **[samp-injector.dll](https://github.com/spc-samp/samp-injector/releases/tag/dll)**: Libreria di [SPC](https://github.com/spc-samp), per l'inizializzazione e l'iniezione di SA-MP | open.mp.
- **[SAMPQuery](https://github.com/justmavi/sampquery)**: Libreria di [justmavi](https://github.com/justmavi), per l'interrogazione asincrona delle informazioni sui server.
- **[Guna.UI2.WinForms](https://www.nuget.org/packages/Guna.UI2.WinForms/)**: Controlli di interfaccia moderni utilizzati nel launcher.

## Contribuzione

I contributi sono ben accetti! Puoi contribuire all'**Emotion Launcher** tramite:

- Segnalazione di bug e problemi.
- Suggerimenti di nuove funzionalità o miglioramenti.
- Invio di pull request con correzioni o nuove funzionalità.

Assicurati di seguire lo stile di codifica del progetto e di testare le tue modifiche prima di inviare una pull request. I contributi devono essere compatibili con .NET 8.0.

## Supporto

In caso di domande o problemi, contattaci tramite:

- Apertura di un'issue nel [repository GitHub](https://github.com/xWendorion/eLauncher/issues).
- Modulo di contatto sul sito ufficiale: [https://elauncher.site](https://elauncher.site).

## Licenza

Questo progetto, **Emotion Launcher**, è concesso in licenza sotto la **Licenza MIT**, una licenza open-source ampiamente utilizzata e permissiva. Ciò significa che sei libero di:

- Utilizzare, copiare, modificare, unire, pubblicare, distribuire, sublicenziare e/o vendere copie del software;
- A condizione che l'avviso di copyright e la notifica di autorizzazione siano inclusi in tutte le copie o porzioni sostanziali del software.

> [!IMPORTANT]
> Questo software è fornito "così com'è", senza garanzie di alcun tipo, espresse o implicite, incluse, ma non limitate a, le garanzie di commerciabilità, idoneità per uno scopo particolare e non violazione.

Per ulteriori dettagli legali, consulta il file [LICENSE](LICENSE) incluso in questo repository.

## Ringraziamenti

Grazie per aver utilizzato l'**Emotion Launcher**. Il tuo supporto e i tuoi contributi aiutano a mantenere il progetto attivo e in continua evoluzione.