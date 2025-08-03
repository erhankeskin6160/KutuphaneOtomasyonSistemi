# 📚 Kütüphane Otomasyon Sistemi

Bu proje, ASP.NET Core MVC mimarisi ile geliştirilmiş, rol bazlı yetkilendirmeye ve kullanıcı yönetimine sahip bir kütüphane otomasyon sistemidir. Sistem, kitap takibi, ödünç alma-iade işlemleri, kullanıcı kayıtları ve admin paneli gibi fonksiyonları içerir. Öğrenciler, üyeler ve yöneticiler için özel işlemler barındırır.

---

## 🚀 Özellikler

- 🧾 **Üyelik Sistemi & Giriş Çıkış**
  - Kayıt olma, giriş yapma, çıkış yapma
  - Şifreyi unutma ve e-posta ile sıfırlama (Token tabanlı)
  
- 🛡 **Rol Bazlı Yetkilendirme**
  - `Admin`, `Kütüphane Görevlisi` ve `Üye` rolleri
  - Yalnızca adminlerin ulaşabildiği panel
  
- 📚 **Kitap Yönetimi**
  - Kitap ekleme / güncelleme / silme
  - Kategori, yazar, yayın evi gibi ilişkili bilgiler
  
- 🕓 **Ödünç Alma & İade Sistemi**
  - Üyelerin kitap ödünç alma ve teslim tarihlerini takip
  - Teslim edilmeyen kitaplar için uyarı mekanizması
  
- 📊 **Raporlama ve Listeleme**
  - Admin için tüm üyeleri, kitapları, aktif ödünçleri listeleme
  - Geç teslim edilen kitaplar
  
- 🔑 **Şifremi Unuttum Özelliği**
  - Kullanıcılar e-posta adresi ile şifre sıfırlama linki alabilir
  - `GeneratePasswordResetTokenAsync()` ve `Url.Action()` ile işlem yapılır

---

## 🧰 Kullanılan Teknolojiler

| Teknoloji             | Açıklama                         |
|----------------------|----------------------------------|
| ASP.NET Core MVC     | Web uygulama çatısı              |
| Entity Framework Core| ORM ve veritabanı işlemleri      |
| Identity             | Kimlik ve rol yönetimi           |
| MSSQL Server         | Veritabanı sistemi                |
| Bootstrap 5          | UI tasarımı                      |
| MailKit              | SMTP üzerinden e-posta gönderimi |
| LINQ                 | Veri sorguları                   |

---

#
