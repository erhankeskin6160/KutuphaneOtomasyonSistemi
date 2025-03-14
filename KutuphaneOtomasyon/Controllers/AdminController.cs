using KutuphaneOtomasyon.Models;
using KutuphaneOtomasyon.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace KutuphaneOtomasyon.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private int a;

        private AppDbContext dbContext;
        private IsbnService _ısbnService;

        
        public AdminController(AppDbContext appDbContext, IsbnService ısbnService)
        {
            var db = appDbContext;
            dbContext = db;
            _ısbnService = ısbnService;
            
          
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login() { return View(); }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(Admin admin)
        {

            var info = dbContext.Admins.FirstOrDefault(x => x.UserName == admin.UserName && x.Password == admin.Password);

            if (info != null)
            {


                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name,info.UserName)
                };


                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                return RedirectToAction("Index", "Admin");
            }

            else
            {
                ViewBag.ErrorMessage = "Kullanıcı Adı Şifre";
                return View();
            }




        }

        public IActionResult Index()//Bu sayfada admin panelinde kütüphanede kaç kitap var kaçı ödünç verildi kaç üye var gibi istatiksel cardlar koyulacak
                                    //Partial Viewler Index.cshtml eklenecek
        {
            var cat = dbContext.Categories.ToList();
             

           var catss = dbContext.Categories.First().CategoryName;

            
           
            return View(cat);
        }

        public IActionResult Members() //Kütüphanin Üyeleri listeleniyor
        {
            var member = dbContext.Users.ToList();
            return View(member);
        }
        [HttpGet]
        public IActionResult MemberRegistration() //Adminin yetkisiyle kullanıcı kaydı yapılıyor(kayıt yapamayanlar için yetklili kayıt yapıyor)
        {

            return View();
        }
        [HttpPost]
        public IActionResult MemberRegistration(User registeruser)//kayıt bilgileri post şekilde alınıyor
        {

            if (Request.Form.Files.Count > 0)
            {
                var filename = Path.GetFileNameWithoutExtension(Request.Form.Files[0].FileName);
                var extension = Path.GetExtension(Request.Form.Files[0].FileName);
                string path = "wwwroot/User/" + filename + extension;
                Stream stream = new FileStream(path, FileMode.Create);
                Request.Form.Files[0].CopyTo(stream);



                registeruser.UserImg = filename + extension;
            }

            dbContext.Users.Add(registeruser);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult DeleteMember(int? id) //Kullanıcı silme işlemi
        {
            var member = dbContext.Users.ToList();
            var deleteuser = dbContext.Users.Where(x => x.Id == id).FirstOrDefault();
            if (deleteuser != null)
            {
                dbContext.Users.Remove(deleteuser);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(member);
        }


        public IActionResult EditMemberList()
        {
            var memberlist = dbContext.Users.ToList();
            return View(memberlist);
        }


        [HttpGet]
        public IActionResult EditMembers(int id) //Adminin yetkisiyle kullanıcı bilgileri düzenleniyor
        {
            var EditUser = dbContext.Users.Where(x => x.Id == id).FirstOrDefault();
            return View(EditUser);
        }

        [HttpPost]
        public IActionResult EditMembers(User user) //Düzenlenen bilgiler post ediliyor
        {

            var EditUser = dbContext.Users.Where(x => x.Id == user.Id).FirstOrDefault();
            string img = "";
            if (Request.Form.Files.Count >= 0)
            {
                var filename = Path.GetFileNameWithoutExtension(Request.Form.Files[0].FileName);
                var extension = Path.GetExtension(Request.Form.Files[0].FileName);
                string path = "wwwroot/User/" + filename + extension;
                Stream stream = new FileStream(path, FileMode.Create);
                Request.Form.Files[0].CopyTo(stream);
                img = filename + extension;
            }


            EditUser.Name = user.Name;
            EditUser.Surname = user.Surname;
            EditUser.Password = user.Password;
            EditUser.Email = user.Email;
            EditUser.Phone = user.Phone;
            EditUser.Address = user.Address;
            EditUser.Role = user.Role;
            EditUser.UserImg = user.UserImg = img;
            dbContext.SaveChanges();

            return RedirectToAction("Index");

        }


        public IActionResult Books() //Kütüphanedeki kitaplar listeleniyor
        {
           var books = dbContext.Books.Include(author => author.Author).Include(x => x.Category).ToList();
            //Eager Loading yaptık şuan lazzy loading kapalı
            return View(books);
        }



        [HttpGet]
        public IActionResult EditBook(int id)
        {

           
            //var book = dbContext.Books.Include(x=>x.Author).Find(id);
            List<SelectListItem> selectListItems = (from x in dbContext.Categories
                                                    select new SelectListItem
                                                    {
                                                        Text = x.CategoryName,
                                                        Value = x.CategoryId.ToString()
                                                    }).ToList();
            ViewBag.cat = selectListItems;
            var book = dbContext.Books.Include(a => a.Author).Where(x => x.BookId == id).First();//Eager loading ile veriyi çektik
            
            return View(book);

        }


        [HttpPost]

        public IActionResult EditBook(Book book) // Kitap güncelleniyor
        {

             

            var editbook = dbContext.Books.Include(a => a.Author).Include(cat=>cat.Category).Where(x => x.BookId == book.BookId).First();
            var author = book.BookName;
            var isAuthorAvailable = dbContext.Authors.Where(author => author.AuthorName == book.Author.AuthorName).FirstOrDefault();
            string genaterecode = GenarateCategoryCode(editbook.Category.CategoryName);
            string assetnumber = GenareteAssetNumber(8);
            book.ShelfNumber = genaterecode + assetnumber;
            book.AssetNumber = assetnumber;
            if (isAuthorAvailable != null)
            {


                editbook.Author = isAuthorAvailable;


            }
            else
            {
                var newauthor = new Author
                {
                    AuthorName = book.Author.AuthorName

                };
                dbContext.Authors.Add(newauthor);
                editbook.Author = newauthor;
            }
            string ımg = default;
            if (Request.Form.Files.Count > 0)
            {
                var FileName = Path.GetFileNameWithoutExtension(Request.Form.Files[0].FileName);
                var Extension = Path.GetExtension(Request.Form.Files[0].FileName);
                var path = "wwwroot/Resimler/Kitaplar/" + FileName + Extension;
                FileStream stream = new FileStream(path, FileMode.Create);
                Request.Form.Files[0].CopyTo(stream);
                ımg = FileName + Extension;
            }
            else
            {
                ımg = editbook.BookImage;
            }

           
            
            book.AssetNumber = assetnumber;
            book.BookImage = ımg;
            editbook.BookName = book.BookName;
            editbook.ISBN = _ısbnService.GenerateISBN();
            editbook.AssetNumber = assetnumber;
            editbook.ShelfNumber = genaterecode + assetnumber;
            editbook.Publisher = book.Publisher;
            editbook.PublicationYear = book.PublicationYear;
            editbook.CategoryId = book.CategoryId;
            editbook.Author.AuthorName = book.Author.AuthorName;
            editbook.BookImage = book.BookImage;
           







            



            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }

        public string GenareteAssetNumber(int karaktersayısı)
        {
            string shelfnumber = default;
            string[] karakterler = new[] {"A", "B", "C", "Ç", "D", "E", "F", "G", "Ğ", "H", "I", "İ", "J", "K", "L",
                "M", "N", "O", "Ö", "P", "R", "S", "Ş", "T", "U", "Ü", "V", "Y", "Z","1","2","3","4","5","6","7","8","9"};//Amaç Rastgele DemirBas numarası oluşturmak   
            Random rnd = new Random();
            for (int i = 0; i < karaktersayısı; i++)
            {
                shelfnumber += karakterler[rnd.Next(karakterler.Length)];

            }

            return shelfnumber;
        }
        public string GenarateCategoryCode(string kategori)
        {
             
             
            Dictionary<string, string> dictonory = new Dictionary<string, string>()
            {
                {"Bilişim","Q"},
                { "Teknoloji", "T" },
                 { "Roman", "R" },
                { "Tarih", "H" },
                 { "Bilim", "B" },
                { "Sanat", "S" },
                { "Felsefe", "F" },
                { "Edebiyat", "E" }
            };

            string kategorikodu = dictonory.ContainsKey(kategori) ? dictonory[kategori]:"X";
            return kategorikodu;
        }

        [HttpGet]
        public IActionResult AddBook() //Kitap ekleme sayfası (sadece admin kitap ekleyebilir)
        {

            IEnumerable<SelectListItem> selects = (from x in dbContext.Categories
                                                   select new SelectListItem
                                                   {
                                                       Text = x.CategoryName,
                                                       Value = x.CategoryId.ToString(),
                                                       

                                                   }).ToList();
            ViewBag.Categories = selects;

            var addbook = new Book();

            //var add1=dbContext.Books.Include(author => author.Author).First();

            return View();
        }
        [HttpPost]
        public IActionResult AddBook(Book book) //kitap eklenen  bilgileri post  ediliyor veri tabanına kaydediyor
        {
             
            var catname=dbContext.Categories.Where(x=>x.CategoryId==book.CategoryId).Select(x => x.CategoryName).FirstOrDefault();
            string genaterecode = GenarateCategoryCode(catname);
             string assetnumber = GenareteAssetNumber(8);
            book.ShelfNumber = genaterecode+assetnumber;
            book.AssetNumber = assetnumber;

           
            //var addbook = dbContext.Books.Include(author => author.Author).First();
            var isAuthorAvailable = dbContext.Authors.FirstOrDefault(author => author.AuthorName == book.Author.AuthorName);
            if (isAuthorAvailable != null)
            {

                book.Author = isAuthorAvailable;
            }
            else
            {
                dbContext.Authors.Add(book.Author);
            }

            var kitapresim = "";
            if (Request.Form.Files.Count > 0)
            {
                var file = Path.GetFileNameWithoutExtension(Request.Form.Files[0].FileName);
                var ext = Path.GetExtension(Request.Form.Files[0].FileName);
                var path = "wwwroot/Resimler/Kitaplar/" + file + ext;
                FileStream fileStream = new FileStream(path, FileMode.Create);
                Request.Form.Files[0].CopyTo(fileStream);
                kitapresim = file + ext;
            }


            book.BookImage = kitapresim; ;
            if (!ModelState.IsValid)
            {
                if (string.IsNullOrEmpty(book.ISBN))
                {
                    book.ISBN = _ısbnService.GenerateISBN();//Böyle kitap eklenirken ISBN değeri verilmediğinde otomatik olarak ISBN Değerini atayacaktır
                }
               
                dbContext.Books.Add(book);


            }
            var kayıtsayısı = dbContext.SaveChanges();
            if (kayıtsayısı > 0)
            {
                ViewBag.KayıtDurumu = "Kayıt Başarılı";
            }
            else
            {
                ViewBag.KayıtDurumu = "Kayıt Başarısız";
            }


            return RedirectToAction("Index");
        }

        [HttpGet]
        


        public IActionResult DeleteBook(int id)
        {
            var deletebook = dbContext.Books.Where(x => x.BookId == id).FirstOrDefault();
            if (deletebook != null)
            {
                dbContext.Books.Remove(deletebook);
                dbContext.SaveChanges();
                ViewBag.SilmeDurumu = "Silme İşlemi Başarılı";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.SilmeDurumu = "Silme İşlemi Başarısız";
                return View();
            }
        }
        List<(string Key, string Message)> messages = new();
        public IActionResult Category() // Kitap kategorileri admin paneline listeleniyor
        {
            var category = dbContext.Categories.ToList();
            return View(category);

        }
        [HttpGet]
        public IActionResult EditCategory(int id) //Kitap kategori ismi düzenleniyor
        {
            var categoryınfo = dbContext.Categories.SingleOrDefault(x => x.CategoryId == id);
            return View(categoryınfo);
        }
        [HttpPost]
        public IActionResult EditCategory(Category edittcategory)
        {
            var categoryınfo = dbContext.Categories.FirstOrDefault(x => x.CategoryId == edittcategory.CategoryId);


            return View(categoryınfo);
        }

        public IActionResult DeleteCategory(int id) // Kitap kategorisi silinyor
        {

            dbContext.Categories.Remove(dbContext.Categories.Find(id));
            var delete = dbContext.SaveChanges();
            if (delete > 0)
            {
                messages.Add(("succes", "Kategori Başarıyla Silindi"));
            }
            else
            {
                messages.Add(("danger", "Kategori Silinemedi Tekrar Deneyiniz!!"));
            }
            return RedirectToAction("Category");
        }

        [HttpGet]
        public IActionResult AddCategory() //Kategori ekleniyor
        {

            return View();
        }
        [HttpPost]
        public IActionResult AddCategory(Category category)
        {


            dbContext.Categories.Add(category);
            var kayıtbaşarılımı = dbContext.SaveChanges();
            if (kayıtbaşarılımı > 0)
            {
                messages.Add(("success", "Kategori ekleme işlemi başarılı."));

            }
            else
            {
                messages.Add(("danger", "Kategori eklenemedi!"));
            }
            TempData["Messages"] = messages;
            return RedirectToAction("Index");
        }

        public IActionResult AuthorList() //Kitapların yazarları listeleniyor
        {
            var authorlist = dbContext.Authors.Include(x => x.Books).ToList();
            var categorylist = dbContext.Categories.Select(x => new SelectListItem { Text = x.CategoryName, Value = x.CategoryId.ToString() }).ToList();

            ViewBag.CategoryList = categorylist;
            return View(authorlist);
        }
        [HttpGet]
        public IActionResult AddAuthor() //Yazar ekleniyor
        {

            return View();
        }
        [HttpPost]
        public IActionResult AddAuthor(Author author)
        {
            dbContext.Add(author);
            dbContext.SaveChanges();


            return View();

        }

        public IActionResult DeleteAuthor(int id) //Yazar siliniyor veritabanından
        {
            var author = dbContext.Authors.Find(id);
            dbContext.Remove(author);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public IActionResult EditAuthor()
        {
            return View();


        }
        [HttpPost]
        public IActionResult EditAuthor(Author author)//Veri tabanında yazar bilgisini güncelleniyor
        {
            var aut = dbContext.Authors.Find(author.Id);
            aut.AuthorName = author.AuthorName;
            dbContext.SaveChanges();
            return RedirectToAction("Index");


        }

        public IActionResult Notifications() // Kütüphane bildiri sistemi
        {
            var notifacations = dbContext.Notifications.ToList();
            return View(notifacations);
        }
        [HttpGet]
        public IActionResult AddNotifications() //Kütüphane biliri yayınlıyor
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddNotifications(Notifications addnotifications)

        {
            var notifacations = dbContext.Notifications.Add(addnotifications);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult DeleteNotifications(int id)
        {
            var notifacations = dbContext.Notifications.Find(id);
            dbContext.Remove(notifacations);
            dbContext.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult DetailNotifications(Notifications notifications)
        {
            var notifacations = dbContext.Notifications.Find(notifications.Id);
            return View(notifacations);
        }
        public IActionResult EditNotifications(Notifications _notifications)//Bildiri içeriği düzenleniyor
        {
            var notifacations = dbContext.Notifications.Find(_notifications.Id);
            notifacations.Subject = _notifications.Subject;
            notifacations.Content = _notifications.Content;
            notifacations.dateTime = _notifications.dateTime;
            dbContext.SaveChanges();
            return View(notifacations);
        }

    }
}
