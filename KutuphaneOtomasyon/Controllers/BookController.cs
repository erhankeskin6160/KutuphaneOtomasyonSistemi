using KutuphaneOtomasyon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KutuphaneOtomasyon.Controllers
{
    public class BookController: Controller
    {

        AppDbContext context;
        public BookController(AppDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]   
        public IActionResult BookDetail(int id)
        {

            var book = context.Books.Include(x => x.Author).Include(c=>c.Category).Where(book => book.BookId == id).FirstOrDefault();
          
            return View(book);
        }
        public IActionResult SearchBook(string key) 
        
        {
            var searchbook = context.Books.Include(author=>author.Author).Include(category=>category.Category).Where(book => book.BookName.Contains(key)|| book.Author.AuthorName.Contains(key)).ToList();
            return View(searchbook);
        }

        public IActionResult BorrowBook(int id) 
        {
          
            return View();
        }
    }
}
