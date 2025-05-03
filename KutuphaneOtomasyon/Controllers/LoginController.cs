using AutoMapper;
using KutuphaneOtomasyon.Models;
using KutuphaneOtomasyon.Models.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using NETCore.MailKit.Core;
using NETCore.MailKit.Infrastructure.Internal;
using System.Collections.Generic;
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

        private AppDbContext dbContext;
        private readonly IMapper _mapper;
        private readonly GoogleReCaptchaService _googleReCaptcha;


        public LoginController(AppDbContext contex, IEmailService emailService , IMapper mapper, GoogleReCaptchaService googleReCaptcha)
        {
            _contex = contex;
            this.emailService = emailService;
            _mapper = mapper;
            _googleReCaptcha = googleReCaptcha;
        }
        [AllowAnonymous]
        public IActionResult Login()
        {

            return View();
        }
        [AllowAnonymous]
        [HttpGet]
            public IActionResult Index()
            {

                return View();
            }
        [AllowAnonymous]

        [HttpPost]
            public async Task<IActionResult> Index(User user)
            {
                var token = Request.Form["g-recaptcha-response"];
                var isValidCaptcha = await _googleReCaptcha.VerifyToken(token);

            if (isValidCaptcha==false)
            {
                ModelState.AddModelError(string.Empty, "Lütfen Recaptha Doğrulamasını tamamla");
                return View(user);

            }
            else
            {
                var info = _contex.Users.FirstOrDefault(x => x.Email == user.Email && x.Password == user.Password);
                if (info != null)
                {
                    var claim = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, info.Email),
                        new Claim(ClaimTypes.Role, "User"),
                        new Claim(ClaimTypes.NameIdentifier,info.Id.ToString())
                    };
                    var identity = new ClaimsIdentity(claim, "UserCookies");
                    var principal = new ClaimsPrincipal(identity);

                    HttpContext.SignInAsync("UserCookies", principal);
                    return RedirectToAction("Index", "User");
                }
                else
                {
                    ViewData["ErrorLogin"] = "Giriş İşlemi Başarısız";
                    return View();
                }
            }

           
        }
        [HttpGet]
        public IActionResult Register() 
        {
            
            return View();  
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserViewModel userViewModel)
        {
            var isValid = await _googleReCaptcha.VerifyToken(userViewModel.RecaptchaToken);

            if (!isValid)
            {
                ModelState.AddModelError(string.Empty, "Lütfen reCAPTCHA doğrulamasını tamamlayın.");
                return View(userViewModel);
            }

            if (ModelState.IsValid)
            {
                var user = _mapper.Map<User>(userViewModel);

                if (Request.Form.Files.Count > 0)
                {
                    var filename = Path.GetFileNameWithoutExtension(Request.Form.Files[0].FileName);
                    var extension = Path.GetExtension(Request.Form.Files[0].FileName);
                    string path = Path.Combine("wwwroot/User/", filename + extension);

                    using (Stream stream = new FileStream(path, FileMode.Create))
                    {
                        await Request.Form.Files[0].CopyToAsync(stream);
                    }

                    user.UserImg = filename + extension;
                }

                _contex.Users.Add(user);
                await _contex.SaveChangesAsync();

                return RedirectToAction("Index");
            }
            

            return View(userViewModel);
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "User");
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
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email)||isValidEmail(Email)==false)
            {

                ViewBag.Message = "Lütfen E_posta Adresinizi Giriniz";
                return View();  

            }
            var user = _contex.Users.FirstOrDefault(x => x.Email == Email);
            if (user==null)
            {
                ViewBag.Message = "Bu e-posta adresi sistemde kayıtlı değil.";
                return View();
            }


            var callbackUrl = $"{Request.Scheme}://{Request.Host}/Login/ResetPassword?Email={Email}";

            await emailService.SendAsync(Email, "Kütüphane Otomasyon Şifre Sıfırlama",
     $"Şifrenizi sıfırlamak için <a href='{callbackUrl}'>buraya tıklayınız</a> Eğer bu isteği siz yapmadıysanız, lütfen bu e-postayı göz ardı ediniz.  <br><br> \r\n    📚 Kütüphanemizi kullandığınız için teşekkür ederiz!<br><br> \r\n    Saygılar,<br> Kütüphane Yönetimi", true);


            ViewBag.Message = "Şifre sıfırlama linki e-posta adresinize gönderildi.";
 
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult ResetPassword(string Email)
        {
            if (string.IsNullOrEmpty(Email)) 
            {

                return NotFound("Geçersiz İşlem"); 
            }
            ViewBag.Email = Email;


            return View();


        }

        [HttpPost]
        [AllowAnonymous]
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
            catch (Exception e)
            {
                return false;
                throw ;
            }

        }


    }

   
}
