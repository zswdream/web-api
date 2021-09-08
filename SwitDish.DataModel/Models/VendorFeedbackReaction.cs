using SwitDish.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.DataModel.Models
{
    public class VendorFeedbackReaction
    {
        public int VendorFeedbackReactionId { get; set; }
        public int CustomerOrderFeedbackId { get; set; }
        public int CustomerId { get; set; }
        public DateTime DateTime { get; set; }
        public VendorFeedbackReactionType ReactionType { get; set; }
        public virtual CustomerOrderFeedback CustomerOrderFeedback { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
