using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace KutuphaneOtomasyon.Models
{
    public class Author
    {
        public int Id { get; set; }
        
      public  string AuthorName { get; set; }

        public List<Book> Books { get; set; }

       
 
    }
}
