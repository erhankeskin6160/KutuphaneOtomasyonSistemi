using KutuphaneOtomasyon.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NETCore.MailKit.Core;
using NETCore.MailKit.Infrastructure.Internal;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace KutuphaneOtomasyon.Controllers
{


    public class LoginController : Controller
    {
        private readonly AppDbContext _contex;
        private readonly IEmailService emailService;
       

        public LoginController(AppDbContext contex, IEmailService emailService )
        {
            _contex = contex;
            this.emailService = emailService;
            
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
                var claim = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, info.Email),
                    new Claim(ClaimTypes.Role, info.Role),
                    new Claim(ClaimTypes.NameIdentifier,info.Id.ToString())
                };
                var identity = new ClaimsIdentity(claim, "Login");
                var principal = new ClaimsPrincipal(identity);

                HttpContext.SignInAsync("Cookies", principal);
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


        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email)||isValidEmail(Email)==false)
            {

                ViewBag.Message = "Lütfen E_posta Adersinizi Giriniz";
                return View();  

            }
            var user = _contex.Users.FirstOrDefault(x => x.Email == Email);
            if (user==null)
            {
                ViewBag.Message = "Bu e-posta adresi sistemde kayıtlı değil.";
                return View();
            }


            var callbackUrl = $"{Request.Scheme}://{Request.Host}/Login/ResetPassword?Email={Email}";

            await emailService.SendAsync(Email, "Şifre Sıfırlama",
     $"Şifrenizi sıfırlamak için <a href='{callbackUrl}'>buraya tıklayınız</a> Eğer bu isteği siz yapmadıysanız, lütfen bu e-postayı göz ardı ediniz.  <br><br> \r\n    📚 Kütüphanemizi kullandığınız için teşekkür ederiz!<br><br> \r\n    Saygılar,<br> Kütüphane Yönetimi", true);


            ViewBag.Message = "Şifre sıfırlama linki e-posta adresinize gönderildi.";
 
            return View();
        }

        [HttpGet]
        public IActionResult ResetPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email)) 
            {

                return NotFound("Geçersiz İşlem"); }
            ViewBag.Email = Email;


            return View();


        }

        [HttpPost]
        public IActionResult ResetPassword(string Email, string Password)
        {
            var user = _contex.Users.FirstOrDefault(x => x.Email == Email);
            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı.");
            }
            user.Password = Password;
            _contex.SaveChanges();
            TempData["Message"] = "Şifreniz Başarıyla Değiştirildi";
            return RedirectToAction("Login");
        }




        public bool isValidEmail(string email) 
        {
            try
            {
                var mail = new MailAddress(email);
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }

        }


    }

   
}
