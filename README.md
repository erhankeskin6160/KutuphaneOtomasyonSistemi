# 📚 Kütüphane Otomasyon Sistemi

Bu proje, ASP.NET Core MVC mimarisi ile geliştirilmiş, kullanıcı ve rol yönetimi bulunan, kitap takibi, ödünç alma-iade, ceza yönetimi ve e-posta entegrasyonlarını kapsayan bir otomasyon sistemidir. Gelişmiş filtreleme, admin paneli, reCAPTCHA koruması ve arka plan servisleriyle birlikte gerçek dünya senaryolarına uygundur.

---

## 🚀 Temel Özellikler

### 👤 Üye İşlemleri
- Üyelik kaydı, giriş/çıkış
- Şifremi unuttum → Token + e-posta ile sıfırlama
- Kendi ödünç alma geçmişini görüntüleme

### 🛡 Rol Tabanlı Yetkilendirme
- `Admin`, `Görevli`, `Üye` rollerine özel erişim
- Admin paneline sadece yetkili kullanıcılar erişebilir

### 📚 Kitap Yönetimi
- Admin panelinde:
  - Kitap ekleme / silme / güncelleme
  - Yazar, kategori, yayınevi ilişkileri
- Kitap arama ve filtreleme (kitap adı, yazar, ISBN ile)

### 📥 Ödünç Alma / İade
- Üyeler kitap ödünç alabilir
- Süresi geçen kitaplar için otomatik ceza sistemi

### 📈 Raporlama
- Tüm kitap, kullanıcı, ödünç alma ve gecikme verileri listelenebilir
- Aktif ve pasif üyelikler görüntülenebilir

### 🔑 Şifremi Unuttum
- ASP.NET Identity kullanılarak token ile parola sıfırlama yapılır
- `MailKit` üzerinden SMTP ile e-posta gönderimi

---

## ⚙️ Arka Plan Servisleri (Services)

| Dosya                      | Görev Açıklaması |
|---------------------------|------------------|
| `CezaService.cs`          | Gecikmiş kitaplar için ceza oluşturur |
| `CezaArkaPlanService.cs`  | Ceza işlemlerini `BackgroundService` ile sürekli kontrol eder |
| `EmailService.cs`         | Kullanıcılara e-posta gönderimi (şifre sıfırlama, uyarı) |
| `GoogleReCaptchaService.cs` | Kullanıcı form işlemlerinde reCAPTCHA doğrulaması yapar |
| `IsbnService.cs`          | Kitap eklerken ISBN sorgulaması yapar veya otomatik numara üretir |

---

## 🧰 Kullanılan Teknolojiler

| Teknoloji             | Açıklama                         |
|----------------------|----------------------------------|
| ASP.NET Core MVC     | Web uygulama çatısı              |
| Entity Framework Core| ORM / veritabanı işlemleri       |
| Identity             | Kimlik ve rol yönetimi           |
| MailKit              | SMTP e-posta gönderimi           |
| MSSQL Server         | Veritabanı                       |
| LINQ                 | Sorgulama işlemleri              |
| Bootstrap            | Responsive arayüz                |
| Google reCAPTCHA     | Güvenlik                         |
| BackgroundService    | Arka plan işlemleri              |

---

## 🗂️ Proje Yapısı
