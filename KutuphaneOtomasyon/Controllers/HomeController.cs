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
         
            Random rnd = new Random();
            var kitapsayisi = dbcontext.Books.Count();
            var sayi=rnd.Next(kitapsayisi);
            var rastgelekitap = dbcontext.Books.Include(Author => Author.Author).Skip(sayi).Take(4).ToList();
            ViewBag.rastgelekitap=rastgelekitap;
            return View();
        }
    }
}
