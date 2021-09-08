using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Domain.Models
{
    public class Customer
    {
        public long CustomerId { get; set; }
        public long? UserId { get; set; }
        public string Image { get; set; }
    }
}
