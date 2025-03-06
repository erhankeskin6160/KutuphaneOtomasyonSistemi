using KutuphaneOtomasyon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KutuphaneOtomasyon.Controllers
{
    public class BookController: Controller
    {

        AppDbContext context;
        public BookController(AppDbContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]   
        public IActionResult BookDetail(int id)//Kitap bilgisi künyesi 
        {

            var book = context.Books.Include(x => x.Author).Include(c=>c.Category).Where(book => book.BookId == id).FirstOrDefault();
          
            return View(book);
        }
        public IActionResult SearchBook(string key) //Kitap arama sistemi
        
        {
            var searchbook = context.Books.Include(author=>author.Author).Include(category=>category.Category).Where(book => book.BookName.Contains(key)|| book.Author.AuthorName.Contains(key)).ToList();
            ViewBag.searchbook = searchbook;
            if (searchbook.Count==0)
            {

                TempData["Message"] = "Aranan kitap veya yazar bulunamadı.";
                return RedirectToAction("Index","Home");   
            }
            return View(searchbook);
        }

        // Ödünç Alma İşlemi
        [HttpPost]
        public IActionResult Borrow(int bookId) //Kitap Ödünç Alma sistemi
        {

            var userclaim = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (userclaim == null)
            {
                return Unauthorized("Kullanıcı Oturum Açmadı");
            }
            var userıd = Int32.Parse(userclaim.Value);
            var book = context.Books.FirstOrDefault(b => b.BookId == bookId);
            if (book == null) return NotFound("Kitap bulunamadı.");

            var user = context.Users.FirstOrDefault(x => x.Id == userıd);
            if (user == null) return NotFound("Kullanıcı bulunamadı.");

            // Ödünç alma işlemi
            BookLoan bookLoan = new BookLoan
            {
                BookId = bookId,
                UserId = userıd,
                LoanDate = DateTime.Now,
                Status = BookLoan.LoanStatus.Approved,
                
               
            };

            try
            {
                context.BookLoans.Add(bookLoan);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                // Hata mesajını logla
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Veritabanına veri kaydedilirken bir hata oluştu.");
            }

            return RedirectToAction("Index", "User");    
        }
        public IActionResult ReturnBook(int loanıd) 
        {
            var bookloan=context.BookLoans.Include(x=>x.Book).FirstOrDefault(x => x.Id == loanıd);
            if (bookloan!=null)
            {
                bookloan.Status=BookLoan.LoanStatus.Returned;
                bookloan.ReturnDate = DateTime.Now;
                bookloan.Book.Quantity = +1; //İade edilen kitabın stok miktari güncellendi
                context.SaveChanges();
                TempData["SuccesReturnDate"] = "Kitap Başarıyla İade Edildi";


            }
            else
            {
                TempData["ErrorReturnDate"] = "Kitap İade Edilemedi";
            }

            return RedirectToAction("MyBook", "Profile");
        }


    }
}
    

