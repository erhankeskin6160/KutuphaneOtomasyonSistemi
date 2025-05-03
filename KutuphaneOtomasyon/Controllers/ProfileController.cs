using KutuphaneOtomasyon.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
 using System.Security.Claims;
using System.Text.RegularExpressions;

namespace KutuphaneOtomasyon.Controllers
{
     
    [Authorize(AuthenticationSchemes = "UserCookies", Policy = "User")]

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
        /// <summary>
        /// Kullanıcı üzerindeki kitapları listeler
        /// </summary>
        /// <param name="page"></param>
        /// <param name="bookname"></param>
        /// <returns></returns>
        public IActionResult MyBook(int page=1 , string bookname="") 
        {
            int pagesize = 4;//Her sayfaya 4 adet kitap listeler
            var user = User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = Int32.Parse(user.Value);
            ViewBag.bookname=bookname;

            var books=context.BookLoans.Include(x => x.User).Include(x=>x.Book).ThenInclude(x=>x.Category).Where(x=>x.UserId==userId && x.ReturnDate==null).ToList();

            if (!String.IsNullOrEmpty(bookname)) 
            {
                books= books.Where(x=>x.Book.BookName.Contains(bookname,StringComparison.OrdinalIgnoreCase)).ToList();
            }
            var totalbook= books.Count();

            var bookspage = books.Skip((page - 1) * pagesize).Take(pagesize).ToList();//Sayfalama işlemi yaptık
            ViewBag.TotalPage = Math.Ceiling((double)totalbook / pagesize);
            ViewBag.CurrentPage = page;

            return View(books);
        }

        /// <summary>
        /// Kullanıcaya kütüphane duyularını listeler
        /// </summary>
        /// <returns></returns>
        public IActionResult Notifications() 
        {
            var notifacations = context.Notifications.ToList();
            return View(notifacations);
        }

       
         
    }
}
