using System.Text.Json.Serialization;

namespace SwitDish.Common.DomainModels
{
    public class ProductCategory
    {
        public int ProductCategoryId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
    }
}