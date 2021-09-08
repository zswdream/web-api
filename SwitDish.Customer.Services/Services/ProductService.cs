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
    public class ProductService : IProductService
    {
        private readonly IRepository<DataModel.Models.Product> productRepository;
        private readonly IMapper mapper;
        private readonly ILoggerManager logger;
        public ProductService(
            IRepository<DataModel.Models.Product> productRepository,
            IMapper mapper,
            ILoggerManager logger)
        {
            this.productRepository = productRepository;
            this.mapper = mapper;
            this.logger = logger;
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return this.mapper.Map<IEnumerable<Product>>(await this.productRepository.GetAllAsync().ConfigureAwait(false));
        }

        public async Task<Product> GetProductAsync(int productId)
        {
            return this.mapper.Map<Product>(await this.productRepository.GetAsync(productId).ConfigureAwait(false));
        }
       
        public async Task<IEnumerable<Product>> GetProductsAsync(int vendorId)
        {
            return this.mapper.Map<IEnumerable<Product>>(await this.productRepository.GetAsync(d=> d.VendorId == vendorId).ConfigureAwait(false));
        }

        public async Task<Product> InsertProductAsync(Product product)
        {
            var newProduct = this.mapper.Map<DataModel.Models.Product>(product);
            this.productRepository.Insert(newProduct);
            await this.productRepository.SaveChangesAsync().ConfigureAwait(false);
            return this.mapper.Map<Product>(newProduct); ;
        }

        public async Task<Product> UpdateProductAsync(Product product)
        {
            var existingProduct = this.mapper.Map<DataModel.Models.Product>(product);
            this.productRepository.Update(existingProduct);
            await this.productRepository.SaveChangesAsync().ConfigureAwait(false);
            return this.mapper.Map<Product>(existingProduct); ;
        }
    }
}
