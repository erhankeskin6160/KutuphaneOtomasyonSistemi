namespace KutuphaneOtomasyon.Models
{
    public class Category 
    {
        public int CategoryId { get; set; }//Kategori Id

    public string CategoryName { get; set; }//Kategori Adı

      ICollection<Book> Books { get; set; }//Kitaplar   

    }
}
