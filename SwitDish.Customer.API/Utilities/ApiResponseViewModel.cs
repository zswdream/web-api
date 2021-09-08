using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Utilities
{
    public class ApiResponseViewModel
    {
        [JsonPropertyName("error")]
        public bool Error { get; set; }
        [JsonPropertyName("message")]
        public string Message { get; set; }
        [JsonPropertyName("data")]
        public Object Data { get; set; }
    }
}
