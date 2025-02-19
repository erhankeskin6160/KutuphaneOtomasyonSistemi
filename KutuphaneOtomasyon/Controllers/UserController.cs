using KutuphaneOtomasyon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KutuphaneOtomasyon.Controllers
{
    public class UserController : Controller
    {
        AppDbContext dbcontext;
        public UserController(AppDbContext dbcontext)
        {
                 this.dbcontext = dbcontext; 
        }
        [Authorize]
        public IActionResult Index()
        {
            var userclaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            int userıd = Int32.Parse(userclaim.Value);
            var bakiye = dbcontext.Users.FirstOrDefault(u => u.Id ==userıd);
            ViewBag.balance = bakiye.Balance.ToString();

            var toplanalınanankitap = dbcontext.BookLoans.Include(x => x.User).Where(x => x.UserId == userıd).Count().ToString();
            ViewBag.toplanalınankitap = toplanalınanankitap;
            return View(bakiye);
        }

        public IActionResult Profile(int id) 
        {
            var profile = dbcontext.Users.Where(x => x.Id == id).ToList();


            return View(profile);
        }
    }
}
