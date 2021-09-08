namespace SwitDish.Vendor.API.Models
{
    public class BankAccountGetById
    {
        public long accountId { get; set; }
        public string accountName { get; set; }
        public string AccountType { get; set; }
        public int? VendorId { get; set; }
        public string VendorName { get; set; }
        public int? BranchId { get; set; }
        public string BranchName { get; set; }
        public string bankName { get; set; }
        public string sortCode { get; set; }
        public string accountNumber { get; set; }

        public string BanckAccountDate { get; set; }

    }
}
