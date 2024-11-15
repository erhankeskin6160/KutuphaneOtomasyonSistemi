using KutuphaneOtomasyon.Models;
namespace KutuphaneOtomasyon.Models
    
{
   
    public class BookLoan
    {
        public enum LoanStatus
        {
            Pending,    // Beklemede
            Approved,   // Onaylandı
            Returned,   // İade Edildi
            Overdue     // Gecikti
        }
        public int Id{ get; set; }
        public int UserId { get; set; }//Kitabın Kimin ALdığını Tutmak İçin
        public int BookId { get; set; }//Kitap Id

        public DateTime LoanDate { get; set; }//Ödünç Alma Tarihi

        public DateTime DeliveryDate //Teslim Etmesi Gereken Tarih
        {
            get { return LoanDate.AddDays(15); }
            set { }
        }

        public DateTime? ReturnDate
        {
            get
            {
                if (ReturnDate>DeliveryDate)
                {
                     //User.Balance += 10;
                }
              return ReturnDate;
            } set { }
        }//Teslim Edilen Tarihi
            
        public LoanStatus Status { get; set; }//Ödünç Durumu

        public User User { get; set; }
        public Book Book { get; set; }
    }
    //Ödünç Alma Sistemi
}
