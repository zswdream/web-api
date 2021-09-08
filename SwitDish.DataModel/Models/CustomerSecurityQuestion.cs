using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.DataModel.Models
{
    public class CustomerSecurityQuestion
    {
        public int CustomerId { get; set; }
        public int SecurityQuestionId { get; set; }
        public string Answer { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual SecurityQuestion SecurityQuestion { get; set; }
    }
}
