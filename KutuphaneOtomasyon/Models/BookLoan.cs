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
        public int BookId { get; set; }//Kitap Id hangi kitabın alındığı tutmak için

        public DateTime LoanDate { get; set; }//Ödünç Alma Tarihi

        public DateTime DeliveryDate //Teslim Etmesi Gereken Tarih
        {
            get { return LoanDate.AddDays(15); }
            set { }
        }

        private DateTime? _returnDate;
        public DateTime? ReturnDate
        {
            get { return _returnDate; }
            set
            {
                _returnDate = value;
                if (_returnDate.HasValue && _returnDate.Value > DeliveryDate)
                {
                    int overdueDays = (int)(_returnDate.Value - DeliveryDate).TotalDays;

                    PenaltyFee(overdueDays);
                }
            }
        }
        //Teslim Edilen Tarihi

        public LoanStatus Status { get; set; }//Ödünç Durumu

        public User User { get; set; }
        public Book Book { get; set; }


        public void PenaltyFee(int overdueDays) 
        {
            var Total= overdueDays * 10;
            User.Balance += Total;
            
        }
    }
    //Ödünç Alma Sistemi
}
