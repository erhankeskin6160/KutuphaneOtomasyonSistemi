namespace KutuphaneOtomasyon.Models
{
    public class Category 
    {
        //Category Tablosu
        public int CategoryId { get; set; }//Kategori Id

    public string CategoryName { get; set; }//Kategori Adı
        public string CategoryImage {  get; set; }  //Kategorinin Resmini Tutar
      ICollection<Book> Books { get; set; }//Kitaplar   

    }
}
