using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace SwitDish.Customer.API.Models
{
    public class CustomerSecurityQuestionViewModel
    {
        [JsonPropertyName("security_question_id")]
        public long SecurityQuestionId { get; set; }
        [JsonPropertyName("customer_id")]
        public long CustomerId { get; set; }
        [JsonPropertyName("question")]
        public string Question { get; set; }
        [JsonPropertyName("answer")]
        public string Answer { get; set; }
    }
}
