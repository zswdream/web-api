using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Models
{
    public class FoodCategoryViewModel
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("title")]
        public string Name { get; set; }
        [JsonPropertyName("src")]
        public string Source { get; set; }
        [JsonPropertyName("value")]
        public string Value { get; set; }
    }
}
