using SwitDish.DataModel.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace SwitDish.DataModel.Models
{
    public class CustomerDeliveryAddress
    {
        public int CustomerDeliveryAddressId { get; set; }
        public int CustomerId { get; set; }
        public string CompleteAddress { get; set; }
        public string DeliveryArea { get; set; }
        public string Instructions { get; set; }
        public bool IsDeleted { get; set; }
        public CustomerDeliveryAddressType CustomerDeliveryAddressType { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
