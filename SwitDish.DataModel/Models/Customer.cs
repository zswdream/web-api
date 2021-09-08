using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SwitDish.DataModel.Models
{
    public class Customer
    {
        public Customer()
        {
            CustomerBookings = new HashSet<CustomerBooking>();
            CustomerDeliveryAddresses = new HashSet<CustomerDeliveryAddress>();
            CustomerFavouriteVendors = new HashSet<CustomerFavouriteVendor>();
            CustomerSecurityQuestions = new HashSet<CustomerSecurityQuestion>();
        }
        public int CustomerId { get; set; }        
        public int ApplicationUserId { get; set; }
        public virtual ApplicationUser ApplicationUser { get;set;}
        public virtual ICollection<CustomerBooking> CustomerBookings { get; set; }
        public virtual ICollection<CustomerDeliveryAddress> CustomerDeliveryAddresses { get; set; }
        public virtual ICollection<CustomerFavouriteVendor> CustomerFavouriteVendors { get; set; }
        public virtual ICollection<CustomerSecurityQuestion> CustomerSecurityQuestions { get; set; }
    }
}
