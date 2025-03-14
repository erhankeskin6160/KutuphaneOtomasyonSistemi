using KutuphaneOtomasyon.Models;
using Microsoft.EntityFrameworkCore;

namespace KutuphaneOtomasyon.Services
{
    public class Cezaservice
    {
        private readonly AppDbContext context;
      private readonly   ILogger<Cezaservice> _logger;
        public Cezaservice(AppDbContext appDbContext, ILogger<Cezaservice> logger)
        {
            context = appDbContext;
            _logger = logger;
        }

        public void CezaUygula() 
        {
           var deger = context.BookLoans.Include(x=>x.User).Where(x=>x.ReturnDate == null && DateTime.Now > x.DeliveryDate).ToList();

            int ceza = 0;

            if (deger.Count==0)
            {
                _logger.LogInformation("Geç İade Edilen Kitap veya İade Edilmesi Gereken kitap yok");
            }

           
            foreach (var item in deger)
            {

                if (item.Status==BookLoan.LoanStatus.Overdue)
                {
                    _logger.LogInformation($"Kullanıcıya {item.User.Name+ item.User.Surname}için  ceza zaten uygulandı");
                    continue;
                }

                int gecikensüre = Math.Abs( (DateTime.Now - item.DeliveryDate.ToLocalTime()).Days);
                int cezamiktarı = 1  * gecikensüre;
                ceza = ceza + cezamiktarı;
                var user = context.Users.FirstOrDefault(x => x.Id == item.UserId);
                _logger.LogInformation($"Kullanıcı{user.Name}için {ceza} TL ceza uygulandı");
                item.Status = BookLoan.LoanStatus.Overdue;
                item.User.Balance += ceza;
                ceza = 0;

            }


            context.SaveChanges();


             
        }

    }
}
