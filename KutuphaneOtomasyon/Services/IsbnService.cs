namespace KutuphaneOtomasyon.Services
{
    public class IsbnService
    {
        private static Random random = new Random();

        public string GenerateISBN() 
        {
            string Önek = "975";
            string RandomSayısı ="";
            for (int i = 0; i < 9; i++)
            {
                RandomSayısı += random.Next(0, 10).ToString();

            }
            string ISBNnumber = Önek + RandomSayısı;

            int checkDigit = CalculateISBN13CheckDigit(ISBNnumber);

            return ISBNnumber + checkDigit;

        }

        private int CalculateISBN13CheckDigit(string isbn12)
        {
            int sum = 0;
            for (int i = 0; i < isbn12.Length; i++)
            {
                int digit = int.Parse(isbn12[i].ToString());
                sum += (i % 2 == 0) ? digit : digit * 3;
            }

            int remainder = sum % 10;
            int checkDigit = (remainder == 0) ? 0 : 10 - remainder;

            return checkDigit;
        }
    }
}
