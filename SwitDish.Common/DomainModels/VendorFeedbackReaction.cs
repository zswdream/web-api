using SwitDish.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.Common.DomainModels
{
    public class VendorFeedbackReaction
    {
        public int VendorFeedbackReactionId { get; set; }
        public int CustomerOrderFeedbackId { get; set; }
        public int CustomerId { get; set; }
        public DateTime DateTime { get; set; }
        public VendorFeedbackReactionType ReactionType { get; set; }
        public CustomerOrderFeedback CustomerOrderFeedback { get; set; }
        public Customer Customer { get; set; }
    }
}