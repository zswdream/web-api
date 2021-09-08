using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.DataModel.Models
{
    public class CustomerOrderFeedback
    {
        public CustomerOrderFeedback()
        {
            VendorFeedbackReactions = new HashSet<VendorFeedbackReaction>();
        }

        public int CustomerOrderFeedbackId { get; set; }
        public int CustomerId { get; set; }
        public int CustomerOrderId { get; set; }
        public DateTime DateTime { get; set; }
        public int FoodRating { get; set; }
        public int AppRating { get; set; }
        public int DeliveryAgentRating { get; set; }
        public string FoodComments { get; set; }
        public string AppComments { get; set; }
        public string DeliveryAgentComments { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual CustomerOrder CustomerOrder { get; set; }
        public virtual ICollection<VendorFeedbackReaction> VendorFeedbackReactions { get; set; }
    }
}
