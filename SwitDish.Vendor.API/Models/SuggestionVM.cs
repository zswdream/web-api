using System;

namespace SwitDish.Vendor.API.Models
{
    public class SuggestionVM
    {
        public int SuggestionId { get; set; }
        public string SuggestionText { get; set; }
        public string SuggestionBy { get; set; }
        public string SuggestionAbout { get; set; }
        public string CreateddDate { get; set; }
        public Nullable<bool> ByEmployee { get; set; }
    }
}
