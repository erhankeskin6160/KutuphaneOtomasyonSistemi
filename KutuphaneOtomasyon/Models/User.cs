using System.ComponentModel.DataAnnotations.Schema;

namespace KutuphaneOtomasyon.Models
{
   public   class User
    {

        //User Tablosı
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Username { get; set; }
        public DateTime? Birthday { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; } //Telefon NO
        public string Address { get; set; }//Ev Adresi
        public string? Role { get; set; }//Kulanıcının Statüsü,Öğrenci,Akademisyen

        public string UserImg { get; set; }// Kullanıcı Fotoğrafı

        public decimal? Balance { get; set; }// 
    }
}
