namespace SwitDish.Vendor.API.Models
{
    public class VendorEmployeeVM
    {
        public long VendorEmployeeId { get; set; }
        public long UserId { get; set; }
        public long VendorId { get; set; }
        public long BranchId { get; set; }
        public string Name { get; set; }
    }
}
