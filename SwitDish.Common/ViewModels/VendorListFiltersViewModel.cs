using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SwitDish.Common.ViewModels
{
    public class VendorListFiltersViewModel
    {
        public VendorListFiltersViewModel()
        {
            Locations = new List<FilterDetailViewModel>();
            Cuisines = new List<FilterDetailViewModel>();
            Features = new List<FilterDetailViewModel>();
            DeliveryMinutes = new List<FilterDetailViewModel>();
            Categories = new List<FilterDetailViewModel>();
        }

        [JsonPropertyName("total-restaurants")]
        public int TotalVendors { get; set; }
        [JsonPropertyName("locations")]
        public List<FilterDetailViewModel> Locations { get; set; }
        [JsonPropertyName("cuisines")]
        public List<FilterDetailViewModel> Cuisines { get; set; }
        [JsonPropertyName("features")]
        public List<FilterDetailViewModel> Features { get; set; }
        [JsonPropertyName("delivery-times")]
        public List<FilterDetailViewModel> DeliveryMinutes { get; set; }
        [JsonPropertyName("categories")]
        public List<FilterDetailViewModel> Categories { get; set; }
    }

    public class FilterDetailViewModel
    {
        public string name { get; set; }
        public int count { get; set; }
    }
}
