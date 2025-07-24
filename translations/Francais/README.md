# Emotion Launcher

L'**Emotion Launcher (eLauncher)** est un lanceur moderne et dédié pour **San Andreas Multiplayer (SA-MP)** et **Open Multiplayer (open.mp)**. Développé avec un accent sur la stabilité, la convivialité et les fonctionnalités avancées, l'**eLauncher** offre une expérience fluide pour se connecter aux serveurs multijoueurs, gérer les favoris, configurer l'installation de GTA San Andreas et maintenir le lanceur à jour de manière automatique et efficace.

## Langues

- Português: [README](../../)
- Deutsch: [README](../Deutsch/README.md)
- English: [README](../English/README.md)
- Español: [README](../Espanol/README.md)
- Italiano: [README](../Italiano/README.md)
- Polski: [README](../Polski/README.md)
- Русский: [README](../Русский/README.md)
- Svenska: [README](../Svenska/README.md)
- Türkçe: [README](../Turkce/README.md)

## Table des matières

- [Emotion Launcher](#emotion-launcher)
  - [Langues](#langues)
  - [Table des matières](#table-des-matières)
  - [Fonctionnalités](#fonctionnalités)
  - [Prérequis](#prérequis)
  - [Installation et utilisation](#installation-et-utilisation)
  - [Configuration](#configuration)
  - [Comment se connecter à un serveur](#comment-se-connecter-à-un-serveur)
  - [Système de mise à jour](#système-de-mise-à-jour)
  - [Exemples de code](#exemples-de-code)
    - [Logique de connexion aux serveurs (simplifiée)](#logique-de-connexion-aux-serveurs-simplifiée)
    - [Exemple de gestionnaire de configuration](#exemple-de-gestionnaire-de-configuration)
    - [Extrait du système de mise à jour](#extrait-du-système-de-mise-à-jour)
  - [Bibliothèques et dépendances tierces](#bibliothèques-et-dépendances-tierces)
  - [Contribution](#contribution)
  - [Support](#support)
  - [Licence](#licence)
  - [Remerciements](#remerciements)

## Fonctionnalités

- **Navigation multi-source:** Explorez les serveurs favoris enregistrés localement, les serveurs en ligne via l'API Open.MP et les serveurs hébergés personnalisés.
- **Requête d'informations asynchrone:** Récupération efficace et non bloquante des détails du serveur tels que le nom, le mode, la langue, le nombre de joueurs, le ping et l'état du mot de passe.
- **Gestion des favoris:** Ajoutez, supprimez et conservez les serveurs favoris dans un fichier local `servers.ini`.
- **Dialogue de connexion intuitif:** Saisissez votre pseudo et le mot de passe du serveur (si nécessaire), avec persistance automatique du pseudo.
- **Configuration du chemin de GTA San Andreas:** Sélectionnez le dossier d'installation de GTA San Andreas ou téléchargez un paquet préconfiguré avec GTA + SA-MP directement via le lanceur.
- **Mises à jour automatiques:** Vérifie automatiquement les nouvelles versions du lanceur en ligne et permet d'appliquer les mises à jour en un seul clic.
- **Interface avec thème sombre personnalisé:** Design moderne, cohérent et visuellement attrayant dans toutes les fenêtres du lanceur.
- **Gestion robuste des erreurs:** Messages conviviaux pour les problèmes de connectivité, les entrées invalides ou les échecs de mise à jour.

## Prérequis

Pour exécuter l'**Emotion Launcher**, les composants suivants sont requis:

- [.NET Desktop Runtime 8.0 (x86)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Visual C++ Redistributable pour Visual Studio 2015–2022 (x86)](https://aka.ms/vs/17/release/vc_redist.x86.exe)

> [!NOTE]
> Ces composants sont obligatoires pour éviter les erreurs d'exécution.

## Installation et utilisation

1. Téléchargez la dernière version du lanceur depuis la page [Releases](https://github.com/xWendorion/eLauncher/releases) ou le site officiel: [https://elauncher.site](https://elauncher.site).
2. Extrayez le fichier téléchargé dans un répertoire de votre choix.
3. Exécutez le fichier `EmotionLauncher.exe`.
4. Configurez le chemin d'installation de GTA San Andreas en cliquant sur le bouton des paramètres.
5. Parcourez les serveurs disponibles dans les onglets des favoris, des serveurs en ligne ou des serveurs hébergés.
6. Double-cliquez sur un serveur pour vous connecter, en entrant votre pseudo et mot de passe si nécessaire.

## Configuration

L'**Emotion Launcher** utilise deux fichiers de configuration principaux, situés dans le répertoire du lanceur:

- **`config.ini`**: Stocke le chemin d'installation de GTA San Andreas.  
   **Exemple de contenu**:
   ```ini
   gta_path=C:\Program Files\Rockstar Games\GTA San Andreas
   ```
- **`servers.ini`**: Contient la liste des serveurs favoris au format `IP:port`.  
   **Exemple de contenu**:
   ```ini
   127.0.0.1:7777
   play.example.com:7777
   ```

Le chemin de GTA San Andreas peut être configuré directement dans l'interface du lanceur ou modifié manuellement dans le fichier `config.ini`.

## Comment se connecter à un serveur

1. Sélectionnez un serveur dans la liste des favoris, des serveurs en ligne ou des serveurs hébergés.
2. Double-cliquez sur le serveur ou appuyez sur le bouton "Connexion".
3. Si le serveur requiert un mot de passe, une fenêtre de saisie apparaîtra.
4. Entrez votre pseudo (enregistré automatiquement pour les sessions futures).
5. Cliquez sur "Rejoindre" pour lancer le jeu et vous connecter en utilisant `samp-injector.dll`.

## Système de mise à jour

Le lanceur vérifie périodiquement les mises à jour via un manifeste JSON hébergé à l'adresse suivante:

```
https://elauncher.site/api/version/version.json
```

Si une nouvelle version est détectée, le lanceur affiche une notification pour télécharger et installer la mise à jour. Le paquet de mise à jour est téléchargé sous forme de fichier ZIP, extrait après la fermeture du lanceur, remplace les anciens fichiers et redémarre automatiquement l'application.

## Exemples de code

### Logique de connexion aux serveurs (simplifiée)

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

### Exemple de gestionnaire de configuration

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

### Extrait du système de mise à jour

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

## Bibliothèques et dépendances tierces

- **[samp-injector.dll](https://github.com/spc-samp/samp-injector/releases/tag/dll)**: Bibliothèque de [SPC](https://github.com/spc-samp), pour l'initialisation et l'injection de SA-MP | open.mp.
- **[SAMPQuery](https://github.com/justmavi/sampquery)**: Bibliothèque de [justmavi](https://github.com/justmavi), pour la requête asynchrone d'informations sur les serveurs.
- **[Guna.UI2.WinForms](https://www.nuget.org/packages/Guna.UI2.WinForms/)**: Contrôles d'interface modernes utilisés dans le lanceur.

## Contribution

Les contributions sont les bienvenues ! Vous pouvez contribuer à l'**Emotion Launcher** en:

- Signalant des bugs et des problèmes.
- Suggérant de nouvelles fonctionnalités ou améliorations.
- Soumettant des pull requests avec des corrections ou de nouvelles fonctionnalités.

Assurez-vous de suivre le style de codage du projet et de tester vos modifications avant de soumettre une pull request. Les contributions doivent être compatibles avec .NET 8.0.

## Support

Pour toute question ou problème, contactez-nous via:

- Ouvrir une issue dans le [dépôt GitHub](https://github.com/xWendorion/eLauncher/issues).
- Formulaire de contact sur le site officiel: [https://elauncher.site](https://elauncher.site).

## Licence

Ce projet, **Emotion Launcher**, est sous licence **MIT**, une licence open-source largement utilisée et permissive. Cela signifie que vous êtes libre de:

- Utiliser, copier, modifier, fusionner, publier, distribuer, sous-licencier et/ou vendre des copies du logiciel ;
- À condition que l'avis de copyright et la notice de permission soient inclus dans toutes les copies ou parties substantielles du logiciel.

> [!IMPORTANT]
> Ce logiciel est fourni « tel quel », sans garantie d'aucune sorte, expresse ou implicite, y compris, mais sans s'y limiter, les garanties de commercialisation, d'adéquation à un usage particulier et de non-contrefaçon.

Pour plus de détails juridiques, consultez le fichier [LICENSE](LICENSE) inclus dans ce dépôt.

## Remerciements

Merci d'utiliser l'**Emotion Launcher**. Votre soutien et vos contributions aident à maintenir le projet actif et en constante évolution.