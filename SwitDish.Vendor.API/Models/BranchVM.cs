namespace SwitDish.Vendor.API.Models
{
    public class BranchVM
    {
        public long BranchId { get; set; }
        public bool IsHeadQuarters { get; set; }
        public string AddressBrance { get; set; }
        public string Manager { get; set; }
        public string Vendor { get; set; }
        public string Account { get; set; }
        public string DateCreate { get; set; }
        //public string DateCreate { get; set; }
        public long UserId { get; set; }
        public bool IsActive { get; set; }
        public string BranchName { get; set; }
        public string Address { get; set; }
        public string VendorName { get; set; }
        public string Account_Name { get; set; }
        public string ManagerName { get; set; }
        public string Account_Type { get; set; }
        public string Account_Number { get; set; }
    }
}
