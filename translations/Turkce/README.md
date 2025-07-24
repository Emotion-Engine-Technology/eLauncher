# Emotion Launcher

**Emotion Launcher (eLauncher)**, **San Andreas Multiplayer (SA-MP)** ve **Open Multiplayer (open.mp)** için modern ve özel bir başlatıcıdır. Kararlılık, kullanılabilirlik ve gelişmiş özelliklere odaklanarak geliştirilen **eLauncher**, çok oyunculu sunuculara bağlanma, favorileri yönetme, GTA San Andreas kurulumunu yapılandırma ve başlatıcıyı otomatik ve verimli bir şekilde güncel tutma konusunda sorunsuz bir deneyim sunar.

## Diller

- Português: [README](../../)
- Deutsch: [README](../Deutsch/README.md)
- English: [README](../English/README.md)
- Español: [README](../Espanol/README.md)
- Français: [README](../Francais/README.md)
- Italiano: [README](../Italiano/README.md)
- Polski: [README](../Polski/README.md)
- Русский: [README](../Русский/README.md)
- Svenska: [README](../Svenska/README.md)

## İçindekiler

- [Emotion Launcher](#emotion-launcher)
  - [Diller](#diller)
  - [İçindekiler](#i̇çindekiler)
  - [Özellikler](#özellikler)
  - [Gereksinimler](#gereksinimler)
  - [Kurulum ve Kullanım](#kurulum-ve-kullanım)
  - [Yapılandırma](#yapılandırma)
  - [Bir Sunucuya Bağlanma](#bir-sunucuya-bağlanma)
  - [Güncelleme Sistemi](#güncelleme-sistemi)
  - [Kod Örnekleri](#kod-örnekleri)
    - [Sunucu Bağlantı Mantığı (Basitleştirilmiş)](#sunucu-bağlantı-mantığı-basitleştirilmiş)
    - [Yapılandırma Yöneticisi Örneği](#yapılandırma-yöneticisi-örneği)
    - [Güncelleme Sistemi Parçası](#güncelleme-sistemi-parçası)
  - [Üçüncü Taraf Kütüphaneler ve Bağımlılıklar](#üçüncü-taraf-kütüphaneler-ve-bağımlılıklar)
  - [Katkıda Bulunma](#katkıda-bulunma)
  - [Destek](#destek)
  - [Lisans](#lisans)
  - [Teşekkürler!](#teşekkürler)

## Özellikler

- **Çok Kaynaklı Gezinme:** Yerel olarak kaydedilmiş favori sunucuları, Open.MP API'si üzerinden çevrimiçi sunucuları ve özel barındırılan sunucuları keşfedin.
- **Asenkron Bilgi Sorgulama:** Sunucu adı, modu, dili, oyuncu sayısı, ping ve şifre durumu gibi detayların etkin ve engellemesiz alımı.
- **Favori Yönetimi:** Favori sunucuları ekleyin, kaldırın ve yerel bir `servers.ini` dosyasında saklayın.
- **Sezgisel Bağlantı Diyaloğu:** Kullanıcı adınızı ve gerekirse sunucu şifresini girin, kullanıcı adı otomatik olarak kaydedilir.
- **GTA San Andreas Yolu Yapılandırması:** GTA San Andreas kurulum klasörünü seçin veya başlatıcı üzerinden doğrudan GTA + SA-MP içeren önceden yapılandırılmış bir paketi indirin.
- **Otomatik Güncellemeler:** Çevrimiçi olarak yeni başlatıcı sürümlerini otomatik olarak kontrol eder ve tek bir tıklamayla güncellemeleri uygular.
- **Özel Koyu Tema Arayüzü:** Tüm başlatıcı pencerelerinde modern, tutarlı ve görsel olarak çekici bir tasarım.
- **Sağlam Hata İşleme:** Bağlantı sorunları, geçersiz girişler veya güncelleme hataları için kullanıcı dostu mesajlar.

## Gereksinimler

**Emotion Launcher**'ı çalıştırmak için aşağıdaki bileşenler gereklidir:

- [.NET Desktop Runtime 8.0 (x86)](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Visual C++ Redistributable for Visual Studio 2015–2022 (x86)](https://aka.ms/vs/17/release/vc_redist.x86.exe)

> [!NOTE]
> Bu bileşenler, yürütme hatalarını önlemek için zorunludur.

## Kurulum ve Kullanım

1. Başlatıcı'nın en son sürümünü [Releases](https://github.com/xWendorion/eLauncher/releases) sayfasından veya resmi web sitesinden indirin: [https://elauncher.site](https://elauncher.site).
2. İndirilen dosyayı istediğiniz bir dizine çıkarın.
3. `EmotionLauncher.exe` dosyasını çalıştırın.
4. Ayarlar düğmesine tıklayarak GTA San Andreas kurulum yolunu yapılandırın.
5. Favoriler, çevrimiçi sunucular veya barındırılan sunucular sekmelerinde mevcut sunucuları tarayın.
6. Bir sunucuya bağlanmak için sunucuya çift tıklayın, gerekirse kullanıcı adınızı ve şifrenizi girin.

## Yapılandırma

**Emotion Launcher**, başlatıcı dizininde bulunan iki ana yapılandırma dosyası kullanır:

- **`config.ini`**: GTA San Andreas kurulum yolunu saklar.  
   **Örnek içerik**:
   ```ini
   gta_path=C:\Program Files\Rockstar Games\GTA San Andreas
   ```
- **`servers.ini`**: `IP:port` formatında favori sunucuların listesini içerir.  
   **Örnek içerik**:
   ```ini
   127.0.0.1:7777
   play.example.com:7777
   ```

GTA San Andreas yolu, başlatıcı arayüzünden doğrudan yapılandırılabilir veya `config.ini` dosyası manuel olarak düzenlenebilir.

## Bir Sunucuya Bağlanma

1. Favoriler, çevrimiçi sunucular veya barındırılan sunucular listesinden bir sunucu seçin.
2. Sunucuya çift tıklayın veya "Bağlan" düğmesine basın.
3. Sunucu şifre gerektiriyorsa bir giriş penceresi görünecektir.
4. Kullanıcı adınızı girin (gelecek oturumlar için otomatik olarak kaydedilir).
5. Oyunu başlatmak ve `samp-injector.dll` kullanarak bağlanmak için "Katıl" seçeneğine tıklayın.

## Güncelleme Sistemi

Başlatıcı, şu adreste barındırılan bir JSON manifestosu aracılığıyla düzenli olarak güncellemeleri kontrol eder:

```
https://elauncher.site/api/version/version.json
```

Yeni bir sürüm tespit edilirse, başlatıcı güncellemeyi indirmek ve kurmak için bir bildirim görüntüler. Güncelleme paketi bir ZIP dosyası olarak indirilir, başlatıcı kapandıktan sonra çıkarılır, eski dosyaların yerine geçer ve uygulama otomatik olarak yeniden başlatılır.

## Kod Örnekleri

### Sunucu Bağlantı Mantığı (Basitleştirilmiş)

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

### Yapılandırma Yöneticisi Örneği

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

### Güncelleme Sistemi Parçası

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

## Üçüncü Taraf Kütüphaneler ve Bağımlılıklar

- **[samp-injector.dll](https://github.com/spc-samp/samp-injector/releases/tag/dll)**: [SPC](https://github.com/spc-samp) tarafından sağlanan kütüphane, SA-MP | open.mp başlatma ve enjeksiyonu için.
- **[SAMPQuery](https://github.com/justmavi/sampquery)**: [justmavi](https://github.com/justmavi) tarafından sağlanan kütüphane, sunucu bilgilerinin asenkron sorgulanması için.
- **[Guna.UI2.WinForms](https://www.nuget.org/packages/Guna.UI2.WinForms/)**: Başlatıcıda kullanılan modern arayüz kontrolleri.

## Katkıda Bulunma

Katkılar memnuniyetle karşılanır! **Emotion Launcher**'a şu yollarla katkıda bulunabilirsiniz:

- Hataları ve sorunları bildirme.
- Yeni özellikler veya iyileştirmeler önerme.
- Düzeltmeler veya yeni özellikler ile pull request gönderme.

Projenin kodlama stilini takip ettiğinizden ve pull request göndermeden önce değişikliklerinizi test ettiğinizden emin olun. Katkılar .NET 8.0 ile uyumlu olmalıdır.

## Destek

Sorularınız veya sorunlarınız için bizimle şu yollarla iletişime geçebilirsiniz:

- [GitHub deposunda](https://github.com/xWendorion/eLauncher/issues) bir issue açma.
- Resmi web sitesindeki iletişim formu: [https://elauncher.site](https://elauncher.site).

## Lisans

Bu proje, **Emotion Launcher**, yaygın olarak kullanılan ve izin verici bir açık kaynak lisansı olan **MIT Lisansı** altında lisanslanmıştır. Bu, şu hakları tanır:

- Yazılımı kullanma, kopyalama, değiştirme, birleştirme, yayınlama, dağıtma, alt lisans verme ve/veya kopyalarını satma;
- Telif hakkı bildirimi ve lisans izninin tüm kopyalarda veya yazılımın önemli kısımlarında yer alması koşuluyla.

> [!IMPORTANT]
> Bu yazılım "olduğu gibi" sağlanır, açık veya zımni herhangi bir garanti olmaksızın, ticari elverişlilik, belirli bir amaca uygunluk ve ihlal etmeme garantileri dahil ancak bunlarla sınırlı olmamak üzere.

Daha fazla yasal ayrıntı için bu depoda bulunan [LICENSE](LICENSE) dosyasına bakın.

## Teşekkürler!

**Emotion Launcher**'ı kullandığınız için teşekkür ederiz. Desteğiniz ve katkılarınız, projenin aktif kalmasını ve sürekli gelişmesini sağlıyor.