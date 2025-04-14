namespace KutuphaneOtomasyon.Models
{
    public class BookViewModel
    {

        public List<Book> Books { get; set; }
        public List<Category> Categories { get; set; }
         public int? CategoryId{ get; set; }
         public string Shortdate { get; set; }


        public int CurrentPage { get; set; }  
        public int TotalPages { get; set; }  
    }
}
