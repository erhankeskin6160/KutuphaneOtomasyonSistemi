using KutuphaneOtomasyon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KütüphaneOtomasyonSistemi.Controllers
{
    public class HomeController : Controller
    {
        AppDbContext dbcontext;
        public HomeController(AppDbContext dbContext)
        {
            this.dbcontext = dbContext; 
        }
        public IActionResult Index()
        {
           
            
            var filteredBooks = dbcontext.Books
                .Where(x => x.Quantity >= 1)
                .Include(b => b.Author).ToList();

             

            
            var rastgelekitap = filteredBooks
                .OrderBy(x => Guid.NewGuid()) // 
                .Take(4)
                .ToList();

            ViewBag.rastgelekitap = rastgelekitap;
            return View(rastgelekitap);
        }

        public IActionResult Book(int? categoryId,string shortdate= "yeni", int page = 1) 
        {
            int pagesize = 6;

            var books = dbcontext.Books.Include(x => x.Category).Include(x=>x.Author).AsQueryable();

            if (categoryId.HasValue)
            {
                books = books.Where(x => x.CategoryId == categoryId.Value);
            }

            //Sıralama algoritmaso 
            switch (shortdate)
            {
                case "yeni":
                    books = books.OrderByDescending(x => x.BookId);
                    break;
                case "eski":
                    books = books.OrderBy(x => x.BookId);
                    break;
                default:
                    books = books.OrderBy(x => x.BookName);
                    break;



            }

            var totalbooks = books.Count();
            var totalpage = (int)Math.Ceiling(totalbooks / (double)pagesize);
            var bookpage = books.Skip((page - 1) * pagesize).Take(pagesize).ToList();


            var category=dbcontext.Categories.ToList();
            var ViewModel=new BookViewModel
            {
                Books = bookpage,
                Categories = category,
                CurrentPage = page,
                TotalPages = totalpage,
                CategoryId = categoryId,
                Shortdate = shortdate
            };


            return View(ViewModel);
        }


        
    }
}
