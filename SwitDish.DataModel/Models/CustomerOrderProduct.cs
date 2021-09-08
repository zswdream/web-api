using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace SwitDish.DataModel.Models
{
    public class CustomerOrderProduct
    {
        public int CustomerOrderProductId { get; set; }
        public int CustomerOrderId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public virtual CustomerOrder CustomerOrder { get; set; }
        public virtual Product Product { get; set; }
    }
}