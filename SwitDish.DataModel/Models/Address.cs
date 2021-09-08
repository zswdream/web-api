namespace SwitDish.DataModel.Models
{
    public class Address
    {
        public int AddressId { get; set; }
        public string FlatNumber { get; set; }
        public string BuildingNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostCode { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string LocalGovernment { get; set; }
        public bool Active { get; set; }
    }
}