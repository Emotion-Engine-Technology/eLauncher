# Emotion Launcher

El **Emotion Launcher (eLauncher)** es un lanzador moderno y dedicado para **San Andreas Multiplayer (SA-MP)** y **Open Multiplayer (open.mp)**. Desarrollado con un enfoque en estabilidad, usabilidad y funciones avanzadas, el **eLauncher** ofrece una experiencia fluida para conectarse a servidores multijugador, gestionar favoritos, configurar la instalación de GTA San Andreas y mantener el lanzador actualizado de manera automática y eficiente.

## Idiomas

- Português: [README](../../)
- Deutsch: [README](../Deutsch/README.md)
- English: [README](../English/README.md)
- Français: [README](../Francais/README.md)
- Italiano: [README](../Italiano/README.md)
- Polski: [README](../Polski/README.md)
- Русский: [README](../Русский/README.md)
- Svenska: [README](../Svenska/README.md)
- Türkçe: [README](../Turkce/README.md)

## Índice

- [Emotion Launcher](#emotion-launcher)
  - [Idiomas](#idiomas)
  - [Índice](#índice)
  - [Funcionalidades](#funcionalidades)
  - [Requisitos](#requisitos)
  - [Instalación y Uso](#instalación-y-uso)
  - [Configuración](#configuración)
  - [Cómo Conectarse a un Servidor](#cómo-conectarse-a-un-servidor)
  - [Sistema de Actualización](#sistema-de-actualización)
  - [Ejemplos de Código](#ejemplos-de-código)
    - [Lógica de Conexión a Servidores (Simplificada)](#lógica-de-conexión-a-servidores-simplificada)
    - [Ejemplo de Gestor de Configuración](#ejemplo-de-gestor-de-configuración)
    - [Fragmento del Sistema de Actualización](#fragmento-del-sistema-de-actualización)
  - [Bibliotecas y Dependencias de Terceros](#bibliotecas-y-dependencias-de-terceros)
  - [Contribución](#contribución)
  - [Soporte](#soporte)
  - [Licencia](#licencia)
  - [¡Gracias!](#gracias)

## Funcionalidades

- **Navegación Multi-Fuente:** Explora servidores favoritos guardados localmente, servidores en línea a través de la API de Open.MP y servidores personalizados alojados.
- **Consulta Asíncrona de Información:** Recuperación eficiente y no bloqueante de detalles del servidor como nombre, modo, idioma, número de jugadores, ping y estado de contraseña.
- **Gestión de Favoritos:** Añade, elimina y guarda servidores favoritos en un archivo local `servers.ini`.
- **Diálogo de Conexión Intuitivo:** Ingresa tu apodo y la contraseña del servidor (cuando sea necesario), con persistencia automática del apodo.
- **Configuración de la Ruta de GTA San Andreas:** Selecciona la carpeta de instalación de GTA San Andreas o descarga un paquete preconfigurado con GTA + SA-MP directamente desde el lanzador.
- **Actualizaciones Automáticas:** Verifica automáticamente nuevas versiones del lanzador en línea y permite aplicar actualizaciones con un solo clic.
- **Interfaz con Tema Oscuro Personalizado:** Diseño moderno, consistente y visualmente atractivo en todas las ventanas del lanzador.
- **Manejo Robusto de Errores:** Mensajes amigables para problemas de conectividad, entradas inválidas o fallos de actualización.

## Requisitos

Para ejecutar el **Emotion Launcher**, se requieren los siguientes componentes:

- [.NET Desktop Runtime 8.0 (x86)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Visual C++ Redistributable para Visual Studio 2015–2022 (x86)](https://aka.ms/vs/17/release/vc_redist.x86.exe)

> [!NOTE]
> Estos componentes son obligatorios para evitar errores de ejecución.

## Instalación y Uso

1. Descarga la versión más reciente del lanzador desde la página de [Releases](https://github.com/xWendorion/eLauncher/releases) o el sitio web oficial: [https://elauncher.site](https://elauncher.site).
2. Extrae el archivo descargado en un directorio de tu elección.
3. Ejecuta el archivo `EmotionLauncher.exe`.
4. Configura la ruta de instalación de GTA San Andreas haciendo clic en el botón de configuración.
5. Navega por los servidores disponibles en las pestañas de favoritos, servidores en línea o servidores alojados.
6. Haz doble clic en un servidor para conectarte, ingresando tu apodo y contraseña si es necesario.

## Configuración

El **Emotion Launcher** utiliza dos archivos de configuración principales, ubicados en el directorio del lanzador:

- **`config.ini`**: Almacena la ruta de instalación de GTA San Andreas.  
   **Ejemplo de contenido**:
   ```ini
   gta_path=C:\Program Files\Rockstar Games\GTA San Andreas
   ```
- **`servers.ini`**: Contiene la lista de servidores favoritos en el formato `IP:puerto`.  
   **Ejemplo de contenido**:
   ```ini
   127.0.0.1:7777
   play.example.com:7777
   ```

La ruta de GTA San Andreas se puede configurar directamente en la interfaz del lanzador o editando manualmente el archivo `config.ini`.

## Cómo Conectarse a un Servidor

1. Selecciona un servidor de la lista de favoritos, servidores en línea o alojados.
2. Haz doble clic en el servidor o presiona el botón "Conectar".
3. Si el servidor requiere una contraseña, aparecerá una ventana de entrada.
4. Ingresa tu apodo (guardado automáticamente para sesiones futuras).
5. Haz clic en "Unirse" para iniciar el juego y conectarte usando `samp-injector.dll`.

## Sistema de Actualización

El lanzador verifica periódicamente las actualizaciones a través de un manifiesto JSON alojado en:

```
https://elauncher.site/api/version/version.json
```

Si se detecta una nueva versión, el lanzador muestra una notificación para descargar e instalar la actualización. El paquete de actualización se descarga como un archivo ZIP, se extrae después de cerrar el lanzador, reemplaza los archivos antiguos y reinicia la aplicación automáticamente.

## Ejemplos de Código

### Lógica de Conexión a Servidores (Simplificada)

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

### Ejemplo de Gestor de Configuración

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

### Fragmento del Sistema de Actualización

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

## Bibliotecas y Dependencias de Terceros

- **[samp-injector.dll](https://github.com/spc-samp/samp-injector/releases/tag/dll)**: Biblioteca de [SPC](https://github.com/spc-samp), para la inicialización e inyección de SA-MP | open.mp.
- **[SAMPQuery](https://github.com/justmavi/sampquery)**: Biblioteca de [justmavi](https://github.com/justmavi), para la consulta asíncrona de información de servidores.
- **[Guna.UI2.WinForms](https://www.nuget.org/packages/Guna.UI2.WinForms/)**: Controles de interfaz modernos utilizados en el lanzador.

## Contribución

¡Las contribuciones son bienvenidas! Puedes contribuir al **Emotion Launcher** mediante:

- Informar de errores y problemas.
- Sugerir nuevas funciones o mejoras.
- Enviar pull requests con correcciones o nuevas funciones.

Asegúrate de seguir el estilo de codificación del proyecto y probar tus cambios antes de enviar un pull request. Las contribuciones deben ser compatibles con .NET 8.0.

## Soporte

Para preguntas o problemas, contáctanos a través de:

- Abrir una issue en el [repositorio de GitHub](https://github.com/xWendorion/eLauncher/issues).
- Formulario de contacto en el sitio web oficial: [https://elauncher.site](https://elauncher.site).

## Licencia

Este proyecto, **Emotion Launcher**, está licenciado bajo la **Licencia MIT**, una licencia de software libre ampliamente utilizada y permisiva. Esto significa que tienes la libertad de:

- Usar, copiar, modificar, fusionar, publicar, distribuir, sublicenciar y/o vender copias del software;
- Siempre que se incluyan el aviso de copyright y la autorización de la licencia en todas las copias o partes sustanciales del software.

> [!IMPORTANT]
> Este software se proporciona "tal cual", sin garantía de ningún tipo, expresa o implícita, incluidas, pero no limitadas a, las garantías de comercialización, idoneidad para un propósito particular y no infracción.

Para más detalles legales, consulta el archivo [LICENSE](LICENSE) incluido en este repositorio.

## ¡Gracias!

Gracias por usar el **Emotion Launcher**. Tu apoyo y contribuciones ayudan a mantener el proyecto activo y en constante evolución.