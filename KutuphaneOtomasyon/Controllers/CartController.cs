using HtmlTags.Reflection;
using KutuphaneOtomasyon.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KutuphaneOtomasyon.Controllers
{
    public class CartController : Controller
    {
        AppDbContext _context;
        public CartController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ViewCart()
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier);
            var userıd = Convert.ToInt32(user.Value);

            var caritem = _context.CartItems.Include(x => x.Book).ThenInclude(x => x.Author).Where(x => x.UserId ==userıd).ToList();
            return View(caritem);
        }

        [HttpPost]
        public IActionResult AddToCart(int bookid)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier);
            var userıd = Convert.ToInt32(user.Value);
            var book = _context.Books.FirstOrDefault(x => x.BookId == bookid);
            if (book == null)
            {
                return NotFound();
            }
            var CartCount = _context.CartItems.Where(x => x.UserId == userıd).Count();
            if (CartCount >= 3)
            {
                TempData["ErroCart"] = "Sepete en fazla 3 kitap ekleyebilirsin";
                return RedirectToAction();
            }
            var cartitem = new CartItem
            {
                BookId = bookid,
                UserId = userıd
            };

            _context.CartItems.Add(cartitem);
            _context.SaveChanges();

            TempData["SuccesCart"] = "Kitap sepete eklendi";
            return RedirectToAction("Index", "Home");

            return View();
        }
        [HttpPost]
        public IActionResult RemoveToCart(int bookid)
        {
            var user = User.FindFirst(ClaimTypes.NameIdentifier);
            var userıd = Convert.ToInt32(user.Value);

            var cartitem = _context.CartItems.FirstOrDefault(book => book.Id == bookid && book.UserId == userıd);

            if (cartitem != null)
            {
                _context.CartItems.Remove(cartitem);
                _context.SaveChanges();
            }
            return RedirectToAction("Index","Home");


        }

        public IActionResult CheckoutCartItems() 
        {
            var user=User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = Convert.ToInt32(user.Value);

            var cartItems=_context.CartItems.Include(x=>x.Book).Where(x=>x.UserId==userId).ToList();

            foreach (var item in cartItems)
            {
                var loan = new BookLoan
                {
                    UserId = userId,
                    BookId = item.Id,
                    LoanDate = DateTime.Now,
                    DeliveryDate = DateTime.Now.AddDays(15),
                    ReturnDate = null,
                    Status=BookLoan.LoanStatus.Approved
                    
                    
                };
                item.Book.Quantity -= 1;
                
                _context.Add(loan);
                _context.CartItems.Remove(item);
            }
            _context.SaveChanges();
            TempData["CartSucces"] = "Başarıyla Ödünç Alındı";
            return RedirectToAction("Index","Home");
        }







    }
}
