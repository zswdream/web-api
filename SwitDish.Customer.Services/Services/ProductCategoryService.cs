using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Configuration;
using SwitDish.Common.DomainModels;
using SwitDish.Common.Interfaces;
using SwitDish.Customer.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace SwitDish.Customer.Services
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductService productService;
        private readonly IMapper mapper;
        private readonly ILoggerManager logger;
        public ProductCategoryService(
            IProductService productService,
            IMapper mapper,
            ILoggerManager logger)
        {
            this.productService = productService;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<IEnumerable<ProductCategory>> GetProductCategories()
        {
            var allProducts = await this.productService.GetAllProductsAsync().ConfigureAwait(false);

            // Select only product categories with atleast one product
            var productCategories = allProducts.Select(d => d.ProductCategory).GroupBy(d => d.ProductCategoryId).Select(d => d.FirstOrDefault());
            
            return this.mapper.Map<IEnumerable<ProductCategory>>(productCategories);
        }
    }
}
