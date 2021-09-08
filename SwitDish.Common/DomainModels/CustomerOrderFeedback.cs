using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.Common.DomainModels
{
    public class CustomerOrderFeedback
    {
        public int CustomerId { get; set; }
        public int CustomerOrderId { get; set; }
        public DateTime DateTime { get; set; }
        public int FoodRating { get; set; }
        public int AppRating { get; set; }
        public int DeliveryAgentRating { get; set; }
        public string FoodComments { get; set; }
        public string AppComments { get; set; }
        public string DeliveryAgentComments { get; set; }
        public Customer Customer { get; set; }
        public CustomerOrder CustomerOrder { get; set; }
        public List<VendorFeedbackReaction> VendorFeedbackReactions { get; set; }
    }
}
