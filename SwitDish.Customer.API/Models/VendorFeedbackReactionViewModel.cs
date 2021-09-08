using SwitDish.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SwitDish.Customer.API.Models
{
    public class VendorFeedbackReactionViewModel
    {
        [Required]
        public int CustomerOrderFeedbackId { get; set; }
        [Required]
        public int CustomerId { get; set; }
        [Required]
        [RegularExpression(@"\bLike\b|\bDislike\b", ErrorMessage = "Reaction Type can only be Like or Dislike")]
        public string ReactionType { get; set; }
    }
}
