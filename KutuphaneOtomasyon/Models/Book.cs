


using System.Drawing;

namespace KutuphaneOtomasyon.Models
{
    public class Book
    {
        public int BookId { get; set; }//Kitap Id

        public string BookName { get; set; }//Kitap Adı

    /*    public string Author { get; set; }*/ //Yazar

        public string? Description { get; set; }//Açıklama
        public string ISBN { get; set; }   //ISBN

        public string AssetNumber { get; set ; }//Demirbaş Numarası

        public string Publisher { get; set; }//Yayınevi

       public string? ShelfNumber { get; set; }  //Raf Numarası  raf numarası kategory ve demirbaşa ismine göre alacak

       public int? Quantity { get; set; } // Adet
        public int PublicationYear { get; set; }//Yayın Yılı

        public string ?BookImage { get; set; }
        public int CategoryId { get; set; } //Kategory Id forgein key
         public Category Category { get; set; }//Kategori Navigasyon Property

         public int AuthorId { get; set; }
        public Author Author { get; set; }




    }

}
