using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KutuphaneOtomasyon.Models
{
   public   class User
    {

        //User Tablosı
        public int Id { get; set; }

        [Required(ErrorMessage ="Lütfen Name Giriniz")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Lütfen Surname Giriniz")]
        public string Surname { get; set; }
        public string? Username { get; set; }
        public DateTime? Birthday { get; set; }
       
        [Required(ErrorMessage = "Lütfen Şifrenizi Girin")]
        public string Password { get; set; }
        [Required(ErrorMessage ="Lütfen E posta adresini doğru bir şekilde giriniz")]
        [EmailAddress] 
        public string Email { get; set; }
        [Required (ErrorMessage ="Lüfen Telefon Numarınızı Giriniz")]
        [Phone]
        public string Phone { get; set; } //Telefon NO
        [Required (ErrorMessage ="Lüfen Adresinizi Giriniz")]
        
        public string Address { get; set; }//Ev Adresi
        public string? Role { get; set; }//Kulanıcının Statüsü,Öğrenci,Akademisyen
        [Required(ErrorMessage ="Lütfen Fotoğrafınızı Yükleyiniz")]
        public string? UserImg { get; set; }// Kullanıcı Fotoğrafı

        public decimal? Balance { get; set; }// 
    }
}
