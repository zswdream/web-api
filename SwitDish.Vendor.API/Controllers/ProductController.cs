using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using SwitDish.DataModel_OLD.Models;
using SwitDish.Vendor.API.Models;

namespace SwitDish.Vendor.API.Controllers
{
    public class ProductController : ControllerBase
    {
        private readonly SwitDishDatabaseContext _dbContext;
        private readonly IWebHostEnvironment host;
        private readonly string filePath;
        public ProductController(SwitDishDatabaseContext dbContext, IWebHostEnvironment host)
        {
            _dbContext = dbContext;
            this.host = host;
            filePath = Path.Combine(host.WebRootPath, "Image");
        }
        [Route("Products/GetProducts")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetProducts()
        {
            var list = new List<Product>();
            try
            {
                list = _dbContext.Products.ToList();
                List<ProductVM> listVM = new List<ProductVM>();
                foreach (var item in list)
                {
                    ProductVM obj = new ProductVM();
                    obj.ProductId = item.ProductId;
                    obj.Name = item.Name;
                    obj.Description = item.Description;
                    obj.Price = item.Price;
                    obj.CategoryId = item.CategoryId;
                    obj.CategoryName = _dbContext.Categories.Where(a=>a.CategoryId == obj.CategoryId).Select(b=>b.Name).FirstOrDefault();
                    obj.CreateDate =item.CreatedDate.ToString("M/d/yyyy");
                    var imagePath = filePath + item.Image;
                    byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath); 
                    string imageBase64 = Convert.ToBase64String(imageBytes);
                    obj.Image = "data:image/jpeg;base64," + imageBase64;
                    listVM.Add(obj);
                }
                return this.Ok(listVM);
            }
            catch (Exception ex)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(ex.Message)
                };
                return this.Ok(response);
            }
        }

        [Route("Products/SaveOrUpdate")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveOrUpdate(Product product)
        {
            try
            {
                var list = new List<Product>();
                if (product.ProductId > 0)
                {
                    var productExist = _dbContext.Products.Where(a => a.Name == product.Name).FirstOrDefault();
                    if (productExist == null)
                    {

                        product.CreatedDate = DateTime.Now;
                        if (product.Image.Length > 100)
                        {
                            product.Image = _dbContext.Products.Where(a => a.ProductId == product.ProductId).FirstOrDefault().Image;
                        }
                        var local = _dbContext.Set<Product>()
                             .Local
                             .FirstOrDefault(f => f.ProductId == product.ProductId);
                        if (local != null)
                        {
                            _dbContext.Entry(local).State = EntityState.Detached;
                        }
                        _dbContext.Entry(product).State = EntityState.Modified;
                        _dbContext.SaveChanges();
                        return this.Ok("Success");
                    }
                    else
                    {
                        return this.Ok("Already Exist");
                    }
                   
                }
                else
                {
                    var productExist = _dbContext.Products.Where(a=>a.Name == product.Name).FirstOrDefault();
                    if (productExist == null)
                    {
                        product.CreatedDate = DateTime.Now;
                        _dbContext.Products.Add(product);
                        _dbContext.SaveChanges();
                        return this.Ok("Success");
                    }
                    else
                    {
                        return this.Ok("Already Exist");
                    }
                }
              
            }
            catch (Exception ex)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(ex.Message)
                };
                return this.Ok(response);
            }
        }

        [Route("Products/DeleteById")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteById(Product product)
        {
            try
            {
                var list = new List<Product>();
                product = _dbContext.Products.Where(a => a.ProductId == product.ProductId).FirstOrDefault();
                _dbContext.Products.Remove(product);
                _dbContext.SaveChanges();
                list = _dbContext.Products.ToList();
                List<ProductVM> listVM = new List<ProductVM>();
                foreach (var item in list)
                {
                    ProductVM obj = new ProductVM();
                    obj.ProductId = item.ProductId;
                    obj.Name = item.Name;
                    obj.Description = item.Description;
                    obj.Price = item.Price;
                    obj.CategoryId = item.CategoryId;
                    obj.CreateDate = item.CreatedDate.ToString("M/d/yyyy");
                    var imagePath = filePath + item.Image;
                    byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath);
                    string imageBase64 = Convert.ToBase64String(imageBytes);
                    obj.Image = "data:image/jpeg;base64," + imageBase64;
                    listVM.Add(obj);
                }
                return this.Ok(listVM);
            }
            catch (Exception ex)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(ex.Message)
                };
                return this.Ok(response);
            }
        }
        //save image to image folder
        [HttpPost]
        [Route("Products/UploadImage")]
        public async Task<string> UploadImage(IFormFile uploadedFile)
        {
            string imageName = null;
            var httpRequest = HttpContext.Request;
            //Upload Image
            var postedFile = uploadedFile;

            //Create custom filename
            imageName = new String(Path.GetFileNameWithoutExtension(postedFile.FileName).Take(10).ToArray()).Replace(" ", "-");
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(postedFile.FileName);
            bool exists = System.IO.Directory.Exists(this.filePath);
            if (!exists)
                System.IO.Directory.CreateDirectory(this.filePath);
            var filePath = Path.Combine(this.filePath,imageName);

            using (var stream = System.IO.File.Create(filePath))
            {
                await postedFile.CopyToAsync(stream).ConfigureAwait(false);
            }
            
            return imageName;
        }
    }
}