using KutuphaneOtomasyon.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using System.Security.Claims;

namespace KutuphaneOtomasyon.Controllers
{
    public class BookController : Controller
    {

        AppDbContext context;
        public BookController(AppDbContext context)
        {
            this.context = context;
        }



        public async Task<IActionResult> GetBookByISBN(string isbn)
        {
            string apiUrl = $"https://openlibrary.org/api/books?bibkeys=ISBN:{isbn}&format=json&jscmd=data";


            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(apiUrl);
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var book = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(json);

                    if (book.Count == 0)
                    {
                        TempData["Error"] = "Bu ısbn ile kitap bulunamadı";
                        return RedirectToAction("Index");
                    }

                    var bookInfo = book[$"ISBN:{isbn}"];

                    string AuthorFullName = bookInfo.authors[0].name;
                    var author = context.Authors.FirstOrDefault(x => x.AuthorName == AuthorFullName);
                    if (author == null)
                    {
                        Author author1 = new Author
                        {
                            AuthorName = AuthorFullName
                        };
                        context.Authors.Add(author1);
                        context.SaveChanges();
                        author = author1;
                    }

                    string year = bookInfo.publish_date;

                    string title = bookInfo.title;  // Kitap adı
                    string description = bookInfo.description ?? "Açıklama bulunamadı.";  // Açıklama (varsa)
                    string bookImage = bookInfo.cover?.large ?? "/images/default-cover.jpg";  // Kapak resmi

                    Book newbook = new Book
                    {
                        BookName = bookInfo.title,
                        AuthorId = author.Id,
                        ISBN = isbn,
                        PublicationYear = Convert.ToInt32(year.Substring(year.Length - 4)),
                        BookImage = bookInfo.cover?.large ?? "/images/default-cover.jpg",
                        CategoryId = 1,
                        Description = description,
                        Publisher = ""



                    };
                    context.Books.Add(newbook);
                    context.SaveChanges();
                    TempData["SuccesAddBook"] = "Kitap Başarıyla Eklendi";
                    return RedirectToAction("Index");

                }
                else
                {
                    TempData["ErrorAddBook"] = "Apı ile bağlantı kurulamadı";
                    return RedirectToAction("Index");


                }
            }


        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult BookDetail(int id)//Kitap bilgisi künyesi 
        {

            var book = context.Books.Include(x => x.Author).Include(c => c.Category).Where(book => book.BookId == id).FirstOrDefault();

            return View(book);
        }
        /// <summary>
        /// Kitap ARrama Sistemi
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public IActionResult SearchBook(string key) //Kitap arama sistemi

        {
            var searchbook = context.Books.Include(author => author.Author).Include(category => category.Category).Where(book => book.BookName.Contains(key) || book.Author.AuthorName.Contains(key)).ToList();
            ViewBag.searchbook = searchbook;
            if (searchbook.Count == 0)
            {

                TempData["Message"] = "Aranan kitap veya yazar bulunamadı.";
                return RedirectToAction("Index", "Home");
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
            int booktotal = context.BookLoans.Where(x => x.UserId == userıd && x.Status != BookLoan.LoanStatus.Returned).Count();

            var bookstock = context.Books.FirstOrDefault(book => book.BookId == bookId);

            if (bookstock == null || bookstock.Quantity <= 0)
            {
                TempData["KitapYok"] = "Aranan Kitabın Stoğu kalmamış veya aranan kitap yok";
                return RedirectToAction("Mybook", "Profile");

            }

            if (booktotal >= 3)
            {
                TempData["ErrorBookTotal"] = "En Fazla 3 Adet Ödünç Kitap Alabilirsiniz";
                return RedirectToAction("Mybook", "Profile");
            }




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
                book.Quantity -= 1;
                context.BookLoans.Add(bookLoan);
                context.SaveChanges();
                TempData["SuccesBook"] = "Kitap Başarıyla Ödün Alındı";
            }
            catch (Exception ex)
            {
           
                Console.WriteLine(ex.Message);
                return StatusCode(500, "Veritabanına veri kaydedilirken bir hata oluştu.");
            }

            return RedirectToAction("Index", "User");
        }
        public IActionResult ReturnBook(int loanıd) // Kitap İade İşlemi
        {
            var bookloan = context.BookLoans.Include(x => x.Book).FirstOrDefault(x => x.Id == loanıd);
            if (bookloan != null)
            {
                bookloan.Status = BookLoan.LoanStatus.Returned;
                bookloan.ReturnDate = DateTime.Now;
                bookloan.Book.Quantity = +1; //İade edilen kitabın stok miktari güncellendi

                try
                {
                    context.SaveChanges();
                    TempData["SuccesReturnDate"] = "Kitap Başarıyla İade Edildi";
                }
                catch (Exception)
                {
                    TempData["ErrorReturnDate"] = "Kitap İade Edilemedi";
                }




            }
            else
            {
                TempData["ErrorReturnDate"] = "Kitap İade Edilemedi";
            }
            return RedirectToAction("MyBook", "Profile");
        }

        //Sistem kitabı gecikterenleri bulup ceza yapan algoritma yapacağım
        public IActionResult AddLateFees()
        {
            var userıd = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var deger = context.BookLoans.Where(x => x.UserId == userıd && x.ReturnDate == null && DateTime.Now > x.DeliveryDate).ToList();


            int ceza = 0;

            foreach (var item in deger)
            {
                int gecikensüre = (DateTime.Now - item.DeliveryDate).Days;
                int cezamiktarı = 10 * gecikensüre;
                ceza = ceza + cezamiktarı;

                item.Status = BookLoan.LoanStatus.Overdue;
            }

            var user = context.Users.FirstOrDefault(x => x.Id == userıd);
            if (user != null && ceza > 0)
            {
                user.Balance += ceza;
            }

            context.SaveChanges();


            return RedirectToAction("Profile", "User");


        }
    }
}
 
    

