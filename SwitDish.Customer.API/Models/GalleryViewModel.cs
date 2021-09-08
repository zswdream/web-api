using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Models
{
    public class GalleryViewModel
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }
        [JsonPropertyName("img")]
        public string Image { get; set; }
    }
}
