using System;

namespace SwitDish.Vendor.API.Models
{
    public class SaleByLocationVM
    {

        public long SaleByLocationId { get; set; }
        public string Name { get; set; }
        public Nullable<int> TotalSale { get; set; }
        public Nullable<int> AverageSale { get; set; }
        public Nullable<int> LowestSale { get; set; }
        public Nullable<int> HighestSale { get; set; }

    }
}
