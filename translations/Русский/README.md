# Emotion Launcher

**Emotion Launcher (eLauncher)** — это современный и специализированный лаунчер для **San Andreas Multiplayer (SA-MP)** и **Open Multiplayer (open.mp)**. Разработанный с акцентом на стабильность, удобство использования и расширенные функции, **eLauncher** предлагает плавный опыт подключения к многопользовательским серверам, управления избранными, настройки установки GTA San Andreas и автоматического эффективного обновления лаунчера.

## Языки

- Português: [README](../../)
- Deutsch: [README](../Deutsch/README.md)
- English: [README](../English/README.md)
- Español: [README](../Espanol/README.md)
- Français: [README](../Francais/README.md)
- Italiano: [README](../Italiano/README.md)
- Polski: [README](../Polski/README.md)
- Svenska: [README](../Svenska/README.md)
- Türkçe: [README](../Turkce/README.md)

## Содержание

- [Emotion Launcher](#emotion-launcher)
  - [Языки](#языки)
  - [Содержание](#содержание)
  - [Функции](#функции)
  - [Требования](#требования)
  - [Установка и использование](#установка-и-использование)
  - [Конфигурация](#конфигурация)
  - [Как подключиться к серверу](#как-подключиться-к-серверу)
  - [Система обновления](#система-обновления)
  - [Примеры кода](#примеры-кода)
    - [Логика подключения к серверам (упрощённая)](#логика-подключения-к-серверам-упрощённая)
    - [Пример менеджера конфигурации](#пример-менеджера-конфигурации)
    - [Фрагмент системы обновления](#фрагмент-системы-обновления)
  - [Библиотеки и сторонние зависимости](#библиотеки-и-сторонние-зависимости)
  - [Контрибьюция](#контрибьюция)
  - [Поддержка](#поддержка)
  - [Лицензия](#лицензия)
  - [Благодарности](#благодарности)

## Функции

- **Многоисточниковая навигация:** Просматривайте локально сохранённые избранные серверы, онлайн-серверы через API Open.MP и пользовательские хостинговые серверы.
- **Асинхронный запрос информации:** Эффективное и неблокирующее получение данных сервера, таких как название, режим, язык, количество игроков, пинг и статус пароля.
- **Управление избранным:** Добавляйте, удаляйте и сохраняйте избранные серверы в локальный файл `servers.ini`.
- **Интуитивный диалог подключения:** Введите свой никнейм и пароль сервера (при необходимости), с автоматическим сохранением никнейма.
- **Настройка пути GTA San Andreas:** Выберите папку установки GTA San Andreas или загрузите предварительно настроенный пакет GTA + SA-MP напрямую через лаунчер.
- **Автоматические обновления:** Автоматически проверяет наличие новых версий лаунчера онлайн и позволяет применить обновления одним щелчком.
- **Интерфейс с тёмной темой:** Современный, согласованный и визуально привлекательный дизайн во всех окнах лаунчера.
- **Надёжная обработка ошибок:** Понятные сообщения для проблем с подключением, некорректных вводов или сбоев обновления.

## Требования

Для запуска **Emotion Launcher** необходимы следующие компоненты:

- [.NET Desktop Runtime 8.0 (x86)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Visual C++ Redistributable для Visual Studio 2015–2022 (x86)](https://aka.ms/vs/17/release/vc_redist.x86.exe)

> [!NOTE]
> Эти компоненты обязательны для предотвращения ошибок при выполнении.

## Установка и использование

1. Загрузите последнюю версию лаунчера со страницы [Releases](https://github.com/xWendorion/eLauncher/releases) или с официального сайта: [https://elauncher.site](https://elauncher.site).
2. Распакуйте загруженный файл в выбранную директорию.
3. Запустите файл `EmotionLauncher.exe`.
4. Настройте путь установки GTA San Andreas, нажав на кнопку настроек.
5. Просматривайте доступные серверы во вкладках избранных, онлайн-серверов или хостинговых серверов.
6. Дважды щёлкните по серверу, чтобы подключиться, введя никнейм и пароль, если требуется.

## Конфигурация

**Emotion Launcher** использует два основных файла конфигурации, расположенных в директории лаунчера:

- **`config.ini`**: Сохраняет путь установки GTA San Andreas.  
   **Пример содержимого**:
   ```ini
   gta_path=C:\Program Files\Rockstar Games\GTA San Andreas
   ```
- **`servers.ini`**: Содержит список избранных серверов в формате `IP:порт`.  
   **Пример содержимого**:
   ```ini
   127.0.0.1:7777
   play.example.com:7777
   ```

Путь к GTA San Andreas можно настроить непосредственно в интерфейсе лаунчера или вручную отредактировать файл `config.ini`.

## Как подключиться к серверу

1. Выберите сервер из списка избранных, онлайн-серверов или хостинговых серверов.
2. Дважды щёлкните по серверу или нажмите кнопку «Подключиться».
3. Если сервер требует пароль, появится окно ввода.
4. Введите свой никнейм (автоматически сохраняется для будущих сессий).
5. Нажмите «Войти», чтобы запустить игру и подключиться с использованием `samp-injector.dll`.

## Система обновления

Лаунчер периодически проверяет наличие обновлений через JSON-манифест, размещённый по адресу:

```
https://elauncher.site/api/version/version.json
```

Если обнаружена новая версия, лаунчер отображает уведомление о загрузке и установке обновления. Пакет обновления загружается в виде ZIP-файла, распаковывается после закрытия лаунчера, заменяет старые файлы и автоматически перезапускает приложение.

## Примеры кода

### Логика подключения к серверам (упрощённая)

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

### Пример менеджера конфигурации

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

### Фрагмент системы обновления

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

## Библиотеки и сторонние зависимости

- **[samp-injector.dll](https://github.com/spc-samp/samp-injector/releases/tag/dll)**: Библиотека от [SPC](https://github.com/spc-samp) для инициализации и инъекции SA-MP | open.mp.
- **[SAMPQuery](https://github.com/justmavi/sampquery)**: Библиотека от [justmavi](https://github.com/justmavi) для асинхронного запроса информации о серверах.
- **[Guna.UI2.WinForms](https://www.nuget.org/packages/Guna.UI2.WinForms/)**: Современные элементы управления интерфейсом, используемые в лаунчере.

## Контрибьюция

Приветствуется любой вклад! Вы можете внести свой вклад в **Emotion Launcher** через:

- Сообщения об ошибках и проблемах.
- Предложения новых функций или улучшений.
- Отправку pull request с исправлениями или новыми функциями.

Убедитесь, что вы следуете стилю кодирования проекта и тестируете свои изменения перед отправкой pull request. Вклад должен быть совместим с .NET 8.0.

## Поддержка

При возникновении вопросов или проблем свяжитесь с нами через:

- Открытие issue в [репозитории GitHub](https://github.com/xWendorion/eLauncher/issues).
- Форму обратной связи на официальном сайте: [https://elauncher.site](https://elauncher.site).

## Лицензия

Этот проект, **Emotion Launcher**, лицензирован под **лицензией MIT**, широко используемой и разрешительной лицензией с открытым исходным кодом. Это означает, что вы можете:

- Использовать, копировать, изменять, объединять, публиковать, распространять, сублицензировать и/или продавать копии программного обеспечения;
- При условии, что уведомление об авторских правах и разрешение на лицензию включены во все копии или существенные части программного обеспечения.

> [!IMPORTANT]
> Это программное обеспечение предоставляется «как есть», без каких-либо гарантий, явных или подразумеваемых, включая, но не ограничиваясь гарантиями коммерческой ценности, пригодности для конкретной цели и отсутствия нарушений.

Для получения дополнительной юридической информации см. файл [LICENSE](LICENSE) в этом репозитории.

## Благодарности

Спасибо за использование **Emotion Launcher**. Ваша поддержка и вклад помогают проекту оставаться активным и постоянно развиваться.