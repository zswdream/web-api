using SwitDish.Common.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductAsync(int productId);
        Task<IEnumerable<Product>> GetProductsAsync(int vendorId);
        Task<Product> InsertProductAsync(Product product);
        Task<Product> UpdateProductAsync(Product product);
    }
}
