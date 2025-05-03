using KutuphaneOtomasyon.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace KutuphaneOtomasyon.Controllers
{
    [Authorize(AuthenticationSchemes = "UserCookies", Policy = "User")]

    public class MessageController : Controller
    {
        AppDbContext _dbContext;

        public MessageController(AppDbContext dbContext)
        {
            _dbContext=dbContext;
 
            
        }
         
        public IActionResult Index()
        {
            var user=User.FindFirst(ClaimTypes.NameIdentifier);
            var email = user.Subject.Name;
             
            var messages = _dbContext.Messages.Where(x => x.Receiver == email).ToList();
            var sentmessage = _dbContext.Messages.Where(x => x.Sender == email).ToList();
            var ıncomingmessagecount = messages.Count.ToString()??"0";
            var sentmessagecount = sentmessage.Count.ToString()??"0";
           
            HttpContext.Session.SetString("IncomingMessage", ıncomingmessagecount); 
            HttpContext.Session.SetString("SentMessageCount", sentmessagecount);




            return View(messages);
        }

        public IActionResult SentMessage() 
        {
            var user = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
            var email = user.Subject.Name;
        
            var sentmessage = _dbContext.Messages.Where(x => x.Sender == email).ToList();
            var messages = _dbContext.Messages.Where(x => x.Receiver == email).ToList();
            

             
            return View(sentmessage);
        }
        [HttpGet]
        public IActionResult NewMessage()
        {
            return View();
        }
        [HttpPost]
        public IActionResult NewMessage(Message message) 
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier);
            var username = user.Subject.Name;
            message.Sender = username;
            message.DateTime = DateTime.Now;
            _dbContext.Messages.Add(message);
            _dbContext.SaveChanges();
            return RedirectToAction("SentMessage","Message");
        }



    }
}
