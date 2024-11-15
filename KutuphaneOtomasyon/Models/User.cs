using System.ComponentModel.DataAnnotations.Schema;

namespace KutuphaneOtomasyon.Models
{
   public   class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string? Role { get; set; }

        public string UserImg { get; set; }

        public decimal? Balance { get; set; }
    }
}
