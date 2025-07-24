# Emotion Launcher

O **Emotion Launcher (eLauncher)** é um Launcher moderno e dedicado para **San Andreas Multiplayer (SA-MP)** e **Open Multiplayer (open.mp)**. Desenvolvido com foco em estabilidade, usabilidade e funcionalidades avançadas, o **eLauncher** oferece uma experiência fluida para conectar-se a servidores multiplayer, gerenciar favoritos, configurar a instalação do GTA San Andreas e manter o Launcher atualizado de forma automática e eficiente.

## Idiomas

- Deutsch: [README](translations/Deutsch/README.md)
- English: [README](translations/English/README.md)
- Español: [README](translations/Espanol/README.md)
- Français: [README](translations/Francais/README.md)
- Italiano: [README](translations/Italiano/README.md)
- Polski: [README](translations/Polski/README.md)
- Русский: [README](translations/Русский/README.md)
- Svenska: [README](translations/Svenska/README.md)
- Türkçe: [README](translations/Turkce/README.md)

## Índice

- [Emotion Launcher](#emotion-launcher)
  - [Idiomas](#idiomas)
  - [Índice](#índice)
  - [Funcionalidades](#funcionalidades)
  - [Requisitos](#requisitos)
  - [Instalação e Uso](#instalação-e-uso)
  - [Configuração](#configuração)
  - [Como Conectar-se a um Servidor](#como-conectar-se-a-um-servidor)
  - [Sistema de Atualização](#sistema-de-atualização)
  - [Exemplos de Código](#exemplos-de-código)
    - [Lógica de Conexão a Servidores (Simplificada)](#lógica-de-conexão-a-servidores-simplificada)
    - [Exemplo de Gerenciador de Configuração](#exemplo-de-gerenciador-de-configuração)
    - [Trecho do Sistema de Atualização](#trecho-do-sistema-de-atualização)
  - [Bibliotecas e Dependências de Terceiros](#bibliotecas-e-dependências-de-terceiros)
  - [Contribuição](#contribuição)
  - [Suporte](#suporte)
  - [Licença](#licença)
  - [Thanks!](#thanks)

## Funcionalidades

- **Navegação Multi-Fonte:** Explore servidores favoritos salvos localmente, servidores online via API do Open.MP e servidores hospedados personalizados.
- **Consulta Assíncrona de Informações:** Recuperação eficiente e não bloqueante de detalhes do servidor, como nome, modo, idioma, número de jogadores, ping e status de senha.
- **Gerenciamento de Favoritos:** Adicione, remova e persista servidores favoritos em um arquivo local `servers.ini`.
- **Diálogo de Conexão Intuitivo:** Insira seu nickname e senha do servidor (quando necessário), com persistência automática do nickname.
- **Configuração do Caminho do GTA San Andreas:** Selecione a pasta de instalação do GTA San Andreas ou baixe um pacote pré-configurado com GTA + SA-MP diretamente pelo Launcher.
- **Atualizações Automáticas:** Verifica automaticamente novas versões do Launcher online e permite a aplicação de atualizações com um clique.
- **Interface com Tema Escuro Personalizado:** Design moderno, consistente e visualmente agradável em todas as janelas do Launcher.
- **Tratamento de Erros Robusto:** Mensagens amigáveis para problemas de conectividade, entradas inválidas ou falhas de atualização.

## Requisitos

Para executar o **Emotion Launcher**, os seguintes componentes são necessários:

- [.NET Desktop Runtime 8.0 (x86)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Visual C++ Redistributable para Visual Studio 2015–2022 (x86)](https://aka.ms/vs/17/release/vc_redist.x86.exe)

> [!NOTE]
> Esses componentes são obrigatórios para evitar erros de execução.

## Instalação e Uso

1. Baixe a versão mais recente do Launcher na página de [Releases](https://github.com/xWendorion/eLauncher/releases) ou no site oficial: [https://elauncher.site](https://elauncher.site).
2. Extraia o arquivo baixado para um diretório de sua escolha.
3. Execute o arquivo `EmotionLauncher.exe`.
4. Configure o caminho de instalação do GTA San Andreas clicando no botão de configurações.
5. Navegue pelos servidores disponíveis nas abas de favoritos, servidores online ou servidores hospedados.
6. Clique duas vezes em um servidor para conectar, inserindo seu nickname e senha, se necessário.

## Configuração

O **Emotion Launcher** utiliza dois arquivos de configuração principais, localizados no diretório do Launcher:

- **`config.ini`**: Armazena o caminho da instalação do GTA San Andreas.  
   **Exemplo de conteúdo**:
   ```ini
   gta_path=C:\Program Files\Rockstar Games\GTA San Andreas
   ```
- **`servers.ini`**: Contém a lista de servidores favoritos no formato `IP:porta`.  
   **Exemplo de conteúdo**:
   ```ini
   127.0.0.1:7777
   play.example.com:7777
   ```

O caminho do GTA San Andreas pode ser configurado diretamente na interface do Launcher ou editando manualmente o arquivo `config.ini`.

## Como Conectar-se a um Servidor

1. Selecione um servidor na lista de favoritos, servidores online ou hospedados.
2. Clique duas vezes no servidor ou pressione o botão "Conectar".
3. Caso o servidor exija senha, uma janela de entrada será exibida.
4. Insira seu nickname (salvo automaticamente para sessões futuras).
5. Clique em "Entrar" para iniciar o jogo e conectar-se usando o `samp-injector.dll`.

## Sistema de Atualização

O Launcher verifica periodicamente por atualizações por meio de um manifesto JSON hospedado em:

```
https://elauncher.site/api/version/version.json
```

Se uma nova versão for detectada, o Launcher exibe uma notificação para baixar e instalar a atualização. O pacote de atualização é baixado como um arquivo ZIP, extraído após o fechamento do Launcher, substituindo os arquivos antigos e reiniciando o aplicativo automaticamente.

## Exemplos de Código

### Lógica de Conexão a Servidores (Simplificada)

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

### Exemplo de Gerenciador de Configuração

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

### Trecho do Sistema de Atualização

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

## Bibliotecas e Dependências de Terceiros

- **[samp-injector.dll](https://github.com/spc-samp/samp-injector/releases/tag/dll)**: Biblioteca da [SPC](https://github.com/spc-samp), para inicialização e injeção do SA-MP | open.mp.
- **[SAMPQuery](https://github.com/justmavi/sampquery)**: Biblioteca de [justmavi](https://github.com/justmavi), para consulta assíncrona de informações de servidores.
- **[Guna.UI2.WinForms](https://www.nuget.org/packages/Guna.UI2.WinForms/)**: Controles de interface modernos utilizados no Launcher.

## Contribuição

Contribuições são bem-vindas! Você pode colaborar com o **Emotion Launcher** por meio de:

- Relato de bugs e problemas.
- Sugestões de novas funcionalidades ou melhorias.
- Envio de pull requests com correções ou novos recursos.

Certifique-se de seguir o estilo de código do projeto e testar suas alterações antes de enviar um pull request. As contribuições devem ser compatíveis com o .NET 8.0.

## Suporte

Em caso de dúvidas ou problemas, entre em contato por meio de:

- Abertura de uma issue no [repositório GitHub](https://github.com/xWendorion/eLauncher/issues).
- Formulário de contato no site oficial: [https://elauncher.site](https://elauncher.site).

## Licença

Este projeto, **Emotion Launcher**, é licenciado sob a **Licença MIT**, uma licença de software livre e permissiva amplamente utilizada. Isso significa que você tem liberdade para:

- Usar, copiar, modificar, mesclar, publicar, distribuir, sublicenciar e/ou vender cópias do software;
- Desde que o aviso de copyright e a permissão da licença sejam incluídos em todas as cópias ou partes substanciais do software.

> [!IMPORTANT]
> Este software é fornecido "no estado em que se encontra", sem qualquer tipo de garantia, expressa ou implícita, incluindo, mas não se limitando a garantias de comercialização, adequação a um propósito específico e não infração.

Para mais detalhes legais, consulte o arquivo [LICENSE](LICENSE) incluído neste repositório.

## Thanks!

Agradecemos por utilizar o **Emotion Launcher**. Seu apoio e contribuições ajudam a manter o projeto ativo e em constante evolução.