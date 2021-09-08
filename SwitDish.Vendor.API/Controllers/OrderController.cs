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
    public class OrderController : ControllerBase
    {
        private SwitDishDatabaseContext _dbContext;
        public OrderController(SwitDishDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Route("Orders/GetOrders")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrders()
        {
            var list = new List<Order>();
            list = _dbContext.Orders.ToList();
            try
            {
                List<OrderVM> listVM = new List<OrderVM>();
                foreach (var item in list)
                {
                    OrderVM obj = new OrderVM();
                    obj.Amount = item.Amount;
                    //obj.CustomerName = item.CustomerId.ToString();
                    obj.CustomerId = item.CustomerId;
                    obj.CustomerName = string.Empty;
                    var customer = _dbContext.Customers.Where(x => x.CustomerId == obj.CustomerId).ToList().FirstOrDefault();
                    if (customer != null)
                    {
                        var UserId = customer.UserId;
                        var User = _dbContext.Users.Where(x => x.UserId == UserId).ToList().FirstOrDefault();
                        if (User != null)
                        {
                            obj.CustomerName = User.FirstName + " " + User.LastName;
                        }
                    }
                    obj.Date = item.Date.ToString("M/d/yyyy");
                    obj.OrderId = item.OrderId;
                    obj.OrderStatus = item.OrderStatus;
                    obj.PaymentType = item.PaymentType;
                    obj.PaymentTypeName = string.Empty;
                    var paymenttype = _dbContext.PaymentTypes.Where(x => x.PaymentTypeId == obj.PaymentType).ToList().FirstOrDefault();
                    if (paymenttype != null)
                    {
                        obj.PaymentTypeName = paymenttype.PaymentTypeName;
                    }
                    obj.ProductId = item.ProductId;
                    obj.ProductName = string.Empty;
                    var product = _dbContext.Products.Where(x => x.ProductId == obj.ProductId).ToList().FirstOrDefault();
                    if (product != null)
                    {
                        obj.ProductName = product.Name;
                    }
                    obj.Quantity = item.Quantity;
                    obj.Description = item.Description;
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
        [Route("Orders/GetOrdersDetail")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetOrdersDetail()
        {
            var list = new List<Order>();
            list = _dbContext.Orders.ToList();
            try
            {
                var httpRequest = HttpContext.Request;
                var orderId = Convert.ToInt32(httpRequest.Query["orderId"]);
                list = _dbContext.Orders.Where(x=>x.OrderId == orderId).ToList();
                List<OrderVM> listVM = new List<OrderVM>();
                foreach (var item in list)
                {
                    OrderVM obj = new OrderVM();
                    obj.Amount = item.Amount;
                    //obj.CustomerName = item.CustomerId.ToString();
                    obj.CustomerId = item.CustomerId;
                    obj.CustomerName = string.Empty;
                    var customer = _dbContext.Customers.Where(x => x.CustomerId == obj.CustomerId).ToList().FirstOrDefault();
                    if (customer != null)
                    {
                        var UserId =customer.UserId;
                        var User = _dbContext.Users.Where(x => x.UserId == UserId).ToList().FirstOrDefault();
                        if (User != null)
                        {
                            obj.CustomerName = User.FirstName + " " + User.LastName;
                        }
                    }
                    obj.Date = item.Date.ToString("M/d/yyyy");
                    obj.OrderId = item.OrderId;
                    obj.OrderStatus = item.OrderStatus;
                    obj.PaymentType = item.PaymentType;
                    obj.PaymentTypeName = string.Empty;
                    var paymenttype = _dbContext.PaymentTypes.Where(x => x.PaymentTypeId == obj.PaymentType).ToList().FirstOrDefault();
                    if (paymenttype != null)
                    {
                        obj.PaymentTypeName = paymenttype.PaymentTypeName;
                    }
                    obj.ProductId = item.ProductId;
                    obj.ProductName = string.Empty;
                    var product = _dbContext.Products.Where(x => x.ProductId == obj.ProductId).ToList().FirstOrDefault();
                    if (product != null)
                    {
                        obj.ProductName = product.Name;
                    }
                    obj.ClaimId = item.ClaimId;
                    obj.ClaimName = string.Empty;
                    var claim = _dbContext.Claims.Where(x => x.ClaimId == obj.ClaimId).ToList().FirstOrDefault();
                    if (claim != null)
                    {
                        obj.ClaimName = claim.ClaimType;
                    }
                    obj.VendorEmployeeId = item.VendorEmployeeId;
                    obj.VendorEmployeeName = string.Empty;
                    var VendorEmployee = _dbContext.VendorEmployees.Where(x => x.VendorEmployeeId == obj.VendorEmployeeId).ToList().FirstOrDefault();
                    if (VendorEmployee != null)
                    {
                        var UserId = customer.UserId;
                        var User = _dbContext.Users.Where(x => x.UserId == UserId).ToList().FirstOrDefault();
                        if (User != null)
                        {
                            obj.VendorEmployeeName = User.FirstName + " " + User.LastName;
                        }

                    }
                    obj.RewardId = item.RewardId;
                    obj.RewardName = string.Empty;
                    var reward = _dbContext.Rewards.Where(x => x.RewardId == obj.RewardId).ToList().FirstOrDefault();
                    if (reward != null)
                    {
                        obj.RewardName = "Good";
                    }
                    obj.Quantity = item.Quantity;
                    obj.Description = item.Description;
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
        [Route("Orders/SaveStatus")]
        [HttpPost]
        [AllowAnonymous]
        //[Route("api/agent/submit")]
        public async Task<IActionResult> SaveStatus(Order order)
        {
            try
            {
                var list = new List<Order>();
                Order orders = _dbContext.Orders.Where(a => a.OrderId == order.OrderId).FirstOrDefault();
                orders.OrderStatus = order.OrderStatus;
                _dbContext.Entry(orders).State = EntityState.Modified;
                _dbContext.SaveChanges();
                list = _dbContext.Orders.ToList();
                List<OrderVM> listVM = new List<OrderVM>();
                foreach (var item in list)
                {
                    OrderVM obj = new OrderVM();
                    obj.OrderId = item.OrderId;
                    obj.Date = item.Date.ToString("M/d/yyyy");
                    obj.Amount = item.Amount;
                    obj.PaymentType = item.PaymentType;
                    obj.Description = item.Description;
                    obj.VendorEmployeeId = item.VendorEmployeeId;
                    obj.RewardId = item.RewardId;
                    obj.ClaimId = item.ClaimId;
                    obj.CustomerId = item.CustomerId;
                    obj.ProductId = item.ProductId;
                    obj.Quantity = item.Quantity;
                    obj.OrderStatus = item.OrderStatus;
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