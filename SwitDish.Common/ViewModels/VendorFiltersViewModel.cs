using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitDish.Common.ViewModels
{
    public class VendorFiltersViewModel
    {
        public List<string> Locations { get; set; }
        public List<string> Cuisines { get; set; }
        public List<string> Features { get; set; }
        public List<int?> DeliveryMinutes { get; set; }
        public List<string> Categories { get; set; }
        public List<string> ProductCategories { get; set; }
        public List<string> Extras { get; set; }
    }
}
