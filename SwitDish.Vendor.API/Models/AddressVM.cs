using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitDish.Vendor.API.Models
{
    public class AddressVM
    {
        public long AddressId { get; set; }
        public string FlatNumber { get; set; }
        public string BuildingNumber { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string PostCode { get; set; }
        public string County { get; set; }
        public string Country { get; set; }
        public bool? Active { get; set; }
    }
}
