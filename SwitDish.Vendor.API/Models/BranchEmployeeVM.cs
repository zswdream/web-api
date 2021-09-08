namespace SwitDish.Vendor.API.Models
{
    public class BranchEmployeeVM
    {
        public long BranchEmployeeId { get; set; }
        public long BranchId { get; set; }
        public long EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public string EmployeeRole { get; set; }
        public bool selected { get; set; }

    }
}
