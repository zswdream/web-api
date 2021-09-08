namespace SwitDish.Vendor.API.Models
{
    public class BankAccountVM
    {
        public long AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountType { get; set; }
        public int? VendorId { get; set; }
        public string VendorName { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }
        public string BankName { get; set; }
        public string SortCode { get; set; }
        public string AccountNumber { get; set; }
        //public DateTime? Date { get; set; }
        public string BanckAccountDate { get; set; }


    }
}
