using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Models
{
    public class CustomerOrderFeedbackViewModel
    {
        [Required]
        public int CustomerId { get; set; }
        [Required]
        public int CustomerOrderId { get; set; }
        [Range(0,5)]
        public int FoodRating { get; set; }
        [Range(0,5)]
        public int AppRating { get; set; }
        [Range(0,5)]
        public int DeliveryAgentRating { get; set; }
        public string FoodComments { get; set; }
        public string AppComments { get; set; }
        public string DeliveryAgentComments { get; set; }
    }
}
