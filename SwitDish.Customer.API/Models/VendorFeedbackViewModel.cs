using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Models
{
    public class VendorFeedbackViewModel
    {
        public int CustomerOrderId { get; set; }
        public int CustomerId { get; set; }
        public int VendorId { get; set; }
        public string CustomerName { get; set; }
        public DateTime FeedbackDate { get; set; }
        public int Rating { get; set; }
        public string Comments { get; set; }
        public int Likes { get; set; }
        public int Dislikes { get; set; }
    }
}
