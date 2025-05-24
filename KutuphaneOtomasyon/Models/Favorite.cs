namespace KutuphaneOtomasyon.Models
{
    public class Favorite
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }

        public DateTime Favorite_date { get; set; } = DateTime.Now;
        public User User { get; set; }
        public Book Book { get; set; }
    }
     
}
