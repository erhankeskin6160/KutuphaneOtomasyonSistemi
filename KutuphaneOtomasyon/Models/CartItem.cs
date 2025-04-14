namespace KutuphaneOtomasyon.Models
{
    public class CartItem
    {
        public int Id { get; set; }
        public int BookId { get; set; }
       
        public int UserId
        {
            get; set;
        }
       public int Quantity { get; set; }
        public Book Book { get; set; }// Navigation Propertyy

        public User User { get; set; }
    }
}
