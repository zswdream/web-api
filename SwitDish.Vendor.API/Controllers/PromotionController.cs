using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwitDish.DataModel_OLD.Models;
using SwitDish.Vendor.API.Models;

namespace SwitDish.Vendor.API.Controllers
{
    public class PromotionController : ControllerBase
    {
        private SwitDishDatabaseContext _dbContext;
        public PromotionController(SwitDishDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Route("Promotions/GetPromotions")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPromotions()
        {
            var list = new List<Promotion>();
            try
            {
                list = _dbContext.Promotions.ToList();
                List<PromotionVM> listVM = new List<PromotionVM>();
                foreach (var item in list)
                {
                    PromotionVM obj = new PromotionVM();
                    obj.PromotionId = item.PromotionId;
                    obj.Name = item.Name;
                    obj.uniqueReference = item.UniqueReference;
                    obj.DateCreate = item.DateCreated.ToString("M/d/yyyy");
                    obj.DateExpired = item.DateExpired;
                    obj.CreatedBy = item.CreatedBy;
                    obj.isUsed = item.IsUsed;
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

        [Route("Promotions/SaveOrUpdate")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveOrUpdate(Promotion promotion)
        {
            try
            {
                var list = new List<Promotion>();
                if (promotion.PromotionId > 0)
                {
                    promotion.DateCreated = DateTime.Now;
                    promotion.DateExpired = DateTime.Today.AddDays(10);
                    promotion.UniqueReference = Guid.NewGuid().ToString();
                    promotion.CreatedBy = "Umer";
                    _dbContext.Entry(promotion).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    list = _dbContext.Promotions.ToList();
                    List<PromotionVM> listVM = new List<PromotionVM>();
                    foreach (var item in list)
                    {
                        PromotionVM obj = new PromotionVM();
                        obj.PromotionId = item.PromotionId;
                        obj.Name = item.Name;
                        obj.isUsed = item.IsUsed;
                        listVM.Add(obj);
                    }
                    return this.Ok(listVM);

                }
                else
                {
                    promotion.DateCreated = DateTime.Now;
                    promotion.DateExpired = DateTime.Today.AddDays(10);
                    promotion.UniqueReference = "ref";
                    promotion.CreatedBy = "Umer";
                    _dbContext.Promotions.Add(promotion);
                    _dbContext.SaveChanges();
                    list = _dbContext.Promotions.ToList();
                    List<PromotionVM> listVM = new List<PromotionVM>();
                    foreach (var item in list)
                    {
                        PromotionVM obj = new PromotionVM();
                        obj.PromotionId = item.PromotionId;
                        obj.Name = item.Name;
                        obj.isUsed = item.IsUsed;
                        listVM.Add(obj);
                    }
                    return this.Ok(listVM);
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

        [Route("Promotions/DeleteById")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteById(Promotion promotion)
        {
            try
            {
                var list = new List<Promotion>();
                promotion = _dbContext.Promotions.Where(a => a.PromotionId == promotion.PromotionId).FirstOrDefault();
                _dbContext.Promotions.Remove(promotion);
                _dbContext.SaveChanges();
                list = _dbContext.Promotions.ToList();
                List<PromotionVM> listVM = new List<PromotionVM>();
                foreach (var item in list)
                {
                    PromotionVM obj = new PromotionVM();
                    obj.PromotionId = item.PromotionId;
                    obj.Name = item.Name;
                    obj.uniqueReference = item.UniqueReference;
                    obj.DateCreate = item.DateCreated.ToString("M/d/yyyy");
                    obj.DateExpired = item.DateExpired;
                    obj.CreatedBy = item.CreatedBy;
                    obj.isUsed = item.IsUsed;
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
        [Route("Promotions/GetPromotionProduct")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetPromotionProduct()
        {
            var list = new List<PromotionProduct>();
            try
            {
                list = _dbContext.PromotionProducts.ToList();
                List<PromotionProductVM> listVM = new List<PromotionProductVM>();
                foreach (var item in list)
                {
                    PromotionProductVM obj = new PromotionProductVM();
                    obj.Id = item.Id;
                    obj.ProductId = item.ProductId;
                    var product = _dbContext.Products.Where(x => x.ProductId== obj.ProductId).ToList().FirstOrDefault();
                    obj.ProductName = string.Empty;
                    if (product != null)
                    {
                        obj.ProductName = product.Name;
                    }
                    obj.promotionId = item.PromotionId;
                    obj.PromotionName = item.PromotionName;
                    obj.DateExpire = item.DateExpired.ToString("M/d/yyyy");
                    obj.userId = item.UserId;
                    var user = _dbContext.Users.Where(x => x.UserId ==obj.userId).ToList().FirstOrDefault();
                    obj.username = string.Empty;
                    if (user != null)
                    {
                        obj.username = user.Username;
                    }
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
    }
}