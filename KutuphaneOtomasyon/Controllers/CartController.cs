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
            if (userıd==null)
            {
                return NotFound();
            }
            if (book == null)
            {
                return NotFound();
            }
            var userCartItems = _context.CartItems.Where(x => x.UserId == userıd).ToList();
            var existingCartItem = userCartItems.FirstOrDefault(x => x.BookId == bookid);//Sepete eklenen aynı kitapmı aynı kitapsa adetini artır sepette çift gösterme
            var totalQuantityInCart = userCartItems.Sum(x => x.Quantity) + (existingCartItem != null ? 1 : 0);
            ViewBag.totalQuantityInCart = totalQuantityInCart;
            if (totalQuantityInCart >= 3)
            {
                TempData["ErroCart"] = "Sepete en fazla 3 kitap ekleyebilirsin";
                return RedirectToAction("Index", "Home");
            }
            if (existingCartItem!=null)
            {
                 
                existingCartItem.Quantity += 1;
            }

            else 
            { 
            
                var cartitem = new CartItem
            {
                BookId = bookid,
                UserId = userıd,
                 Quantity = 1
            };

            _context.CartItems.Add(cartitem);
            }
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

            var cartitem = _context.CartItems.FirstOrDefault(book => book.BookId == bookid && book.UserId == userıd);

            if (cartitem != null)
            {
                if (cartitem.Quantity>1)
                {
                    cartitem.Quantity -= 1;


                }
                else 
                {
                    _context.CartItems.Remove(cartitem);
                   
                }
                _context.SaveChanges();     

            }
            return RedirectToAction("Index","Home");


        }

        public IActionResult CheckoutCartItems() //Sepetteki kitapları toplu ödünç alıyor
        {
            var user=User.FindFirst(ClaimTypes.NameIdentifier);
            var userId = Convert.ToInt32(user.Value);
            var mybook = _context.BookLoans.Where(x => x.UserId == userId).ToList();//Kullanıcının üzerindeki kitap sayılarını çektil
            var cartItems = _context.CartItems.Include(x => x.Book).Where(x => x.UserId == userId).ToList();//Sepetteki toplam sayı çekmek için listeyi alıypz
            int totalCartBook = cartItems.Sum(x => x.Quantity);//sepetteki toplam kitap adet bilgisini alıyoruz
            int totalbook = mybook.Count + totalCartBook;//seppeteki kitap adet sayısı ile kullanıcının üzerindeki  kitap adet sayısının toplamı alıyoruz


            

            if (totalbook>=3)
            {
                TempData["ErroCart"] = "Sepete aldıklarınız ve üzerinizde  bulunan kitapların toplam adeti 3 geçemez";
                return RedirectToAction("Index", "Cart");
            }

             

            foreach (var item in cartItems)
            {

                if (item.Book.Quantity<=0)//Kitap stoğu yoksa alamasın veya kitap stoğu yoksa listede göstermicem kullanıcı kitabı göremeyecek ayarlanacak o
                {
                    TempData["ErroCart"] = $"{item.Book.BookName} kitap stoktada yok ödünç alamazsın";
                    continue;

                }

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
