using KutuphaneOtomasyon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KutuphaneOtomasyon.Controllers
{
    public class ProfileController : Controller
    {
        AppDbContext context;
        public ProfileController(AppDbContext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult MyBook() 
        {
            var user= User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = Int32.Parse(user.Value);

            var values = context.BookLoans.Include(x=>x.User).Include(x=>x.Book).Where(x=>x.UserId == userId).ToList();
            
           
                return View(values);
             
        }

        public IActionResult Notifications() 
        {
            var notifacations = context.Notifications.ToList();
            return View(notifacations);
        }
    }
}
