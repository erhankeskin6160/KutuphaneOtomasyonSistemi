using KutuphaneOtomasyon.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KutuphaneOtomasyon.Controllers
{

     
public class LoginController : Controller
 {
        AppDbContext _contex;
        public LoginController(AppDbContext contex)
        {
                _contex = contex;
        }

        public IActionResult Login() 
        {

            return View();  
        }
        [HttpGet]
        public IActionResult Index()
        {
             
            return View();
        }

        [HttpPost]  
        public IActionResult Index(User user)
        {
            var info = _contex.Users.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);
            if (info != null)
            {
                var claim=new List<Claim> 
                {
                    new Claim(ClaimTypes.Name, info.Email),
                    new Claim(ClaimTypes.Role, info.Role),
                    new Claim(ClaimTypes.NameIdentifier,info.Id.ToString())
                };
                var identity = new ClaimsIdentity(claim, "Login");
                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync("Cookies",principal);
                return RedirectToAction("Index", "User");
            }
            else
            {
                return View();

            }
        }

        [HttpPost]
        public IActionResult Borrow(int bookId, int userId)
        {
            var book = _contex.Books.FirstOrDefault(b => b.BookId == bookId);
            if (book == null) return NotFound("Kitap bulunamadı.");

            var user = _contex.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return NotFound("Kullanıcı bulunamadı.");

            // Ödünç alma işlemi
            var bookLoan = new BookLoan
            {
                BookId = bookId,
                UserId = userId,
                LoanDate = DateTime.Now,
                Status = BookLoan.LoanStatus.Approved
            };

            _contex.BookLoans.Add(bookLoan);
            _contex.SaveChanges();

            return RedirectToAction("Search", new { query = "" }); // Arama sayfasına dön
        }



    }
}
