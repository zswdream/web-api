using System.Runtime.CompilerServices;

namespace SwitDish.Common.DomainModels
{
    public class CustomerOrderProduct
    {
        public int CustomerOrderProductId { get; set; }
        public int CustomerOrderId { get; set; }
        public int ProductId { get; set; }
        public decimal Quantity { get; set; }
        public CustomerOrder CustomerOrder { get; set; }
        public Product Product { get; set; }
    }
}