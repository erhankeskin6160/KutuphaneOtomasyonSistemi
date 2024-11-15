using KutuphaneOtomasyon.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
                HttpContext.Session.SetString("email", info.Email);
                HttpContext.Session.SetString("name", info.Name);
                return RedirectToAction("Index", "User");
            }
            return View();
        }
    }
}
