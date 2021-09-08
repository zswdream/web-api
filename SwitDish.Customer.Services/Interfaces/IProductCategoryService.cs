using SwitDish.Common.DomainModels;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services.Interfaces
{
    public interface IProductCategoryService
    {
        Task<IEnumerable<ProductCategory>> GetProductCategories();
    }
}
