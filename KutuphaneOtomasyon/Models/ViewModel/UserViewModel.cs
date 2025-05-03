using System.ComponentModel.DataAnnotations;

namespace KutuphaneOtomasyon.Models.ViewModel
{
    public class UserViewModel
    {
       

        [Required(ErrorMessage = "Lütfen Name Giriniz")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Lütfen Surname Giriniz")]
        public string Surname { get; set; }
         

        [Required(ErrorMessage = "Lütfen Şifrenizi Girin")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Lütfen E posta adresini doğru bir şekilde giriniz")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Lüfen Telefon Numarınızı Giriniz")]
        [Phone]
        public string Phone { get; set; } //Telefon NO
        [Required(ErrorMessage = "Lüfen Adresinizi Giriniz")]

        public string Address { get; set; }//Ev Adresi
        public string? Role { get; set; }//Kulanıcının Statüsü,Öğrenci,Akademisyen
         
        public string? UserImg { get; set; }// Kullanıcı Fotoğrafı

        public decimal? Balance { get; set; }// 

        [Required]
        public string RecaptchaToken { get; set; }

        [Required(ErrorMessage = "Lütfen şifrenizi tekrar giriniz")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Şifreler uyuşmuyor")]
        public string ConfirmPassword { get; set; }
    }
}
