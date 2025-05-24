using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KutuphaneOtomasyon.Models
{
    // Yazar Tablosu
    public class Author
    {
        public int Id { get; set; }
        
      public  string AuthorName { get; set; }

     public string ?Image { get; set; } = "/images/default-author.jpg"; // Yazar resmi için varsayılan bir resim yolu



        public List<Book> Books { get; set; }

       
 
    }
}
