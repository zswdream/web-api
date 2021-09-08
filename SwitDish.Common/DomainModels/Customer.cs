using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.Common.DomainModels
{
    public class Customer
    {
        public int CustomerId { get; set; }
        public int ApplicationUserId { get; set; }
        public ApplicationUser ApplicationUser { get;set;}
        public List<CustomerBooking> CustomerBookings { get; set; }
        public List<CustomerDeliveryAddress> CustomerDeliveryAddresses { get; set; }
        public List<CustomerFavouriteVendor> CustomerFavouriteVendors { get; set; }
        public List<CustomerSecurityQuestion> CustomerSecurityQuestions { get; set; }
    }
}
