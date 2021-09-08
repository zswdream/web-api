using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Domain.Models
{
    public class Vendor
    {
        public long VendorId { get; set; }
        public string Name { get; set; }
        public DateTime? DateIncorporated { get; set; }
    }
}
