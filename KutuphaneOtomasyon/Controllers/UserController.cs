using KutuphaneOtomasyon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KutuphaneOtomasyon.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        AppDbContext dbcontext;
        public UserController(AppDbContext dbcontext)
        {
                 this.dbcontext = dbcontext; 
        }
        [Authorize(AuthenticationSchemes = "UserCookies", Policy = "User")]
        public IActionResult Index()
        {
            var userclaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            int userıd = Int32.Parse(userclaim.Value);
            var bakiye = dbcontext.Users.FirstOrDefault(u => u.Id ==userıd);
            ViewBag.balance = bakiye.Balance.ToString();

            var toplanalınanankitap = dbcontext.BookLoans.Include(x => x.User).Where(x => x.UserId == userıd).Count().ToString();
            ViewBag.toplanalınankitap = toplanalınanankitap;

            var userBooks = dbcontext.BookLoans.Include(x => x.Book).ThenInclude(a=>a.Author).Where(x => x.UserId == userıd).ToList();
            ViewData["UserBooks"] = userBooks;
            var userbook = dbcontext.BookLoans.Include(x => x.Book).Where(x => x.UserId == userıd).ToList();


            return View(bakiye);
            }

        [HttpGet]
        public IActionResult Profile() 
        {
            var userclaim=HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            int userıd = int.Parse(userclaim.Value);
           var profile= dbcontext.Users.Find(userıd);


            return View(profile);
        }
        [HttpPost]
        public IActionResult Profile(User userupdate)
        {
            var userclaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            int userıd = int.Parse(userclaim.Value);
            var profile = dbcontext.Users.Find(userıd);

            profile.Email = userupdate.Email;
            profile.Birthday = userupdate.Birthday;
            profile.Phone = userupdate.Phone;
            profile.Password = userupdate.Password;
            
            
          int result=  dbcontext.SaveChanges();
            if (result > 0)
            {
                TempData["SuccesUpdateProfile"] = "Profil Güncelleme İşlemi Başarıyla Yapıldı";
            }
            else
            {
                TempData["ErrorProfile"] = "Profile Güncelleme Sırasında Bir Hatayla Karşılandı";
            }

            return View(profile);
        }
        public IActionResult UserReadBooks()
        {
             
            return PartialView( );
        }

        [HttpGet]
        public IActionResult Payment()
        {
            var UserId =Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = dbcontext.Users.FirstOrDefault(x => x.Id == UserId);
            if (user==null)
            {

            }
            return View(user);  
        }
        [HttpPost]
        public IActionResult Payment( decimal ödenenmiktar) 
        {
            var UserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (ödenenmiktar<0)
            {
                ModelState.AddModelError("Ödenen Miktar", "Ödeme miktarı sıfır veya negatif olamaz");
                return View();
            }

            var user = dbcontext.Users.FirstOrDefault(x => x.Id == UserId);

            if (user==null)
            {
                return NotFound($"Kullanıcı {user.Id} bulunamadı.");

            }

            user.Balance -= ödenenmiktar;

            dbcontext.SaveChanges();
            return RedirectToAction("SuccesPayment", new { userId = user });
        }

        public IActionResult SuccesPayment() 
        {
            var UserId = Int32.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var user = dbcontext.Users.FirstOrDefault(u => u.Id == UserId);
            if (user == null)
            {
                return NotFound($"Kullanıcı {UserId} bulunamadı.");
            }

            return View(user);
        }
        




    }
}
