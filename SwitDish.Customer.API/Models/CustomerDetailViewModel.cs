using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using SwitDish.Common.DomainModels;

namespace SwitDish.Customer.API.Models
{
    public class CustomerDetailViewModel
    {
        [JsonPropertyName("customer_id")]
        public long CustomerId { get; set; }
        [JsonPropertyName("first_name")]
        public string FirstName { get; set; }
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("phone")]
        public string Phone { get; set; }
        [JsonPropertyName("date_of_birth")]
        public string DateOfBirth { get; set; }
        [JsonPropertyName("image")]
        public string Image { get; set; }
    }
}
