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
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private SwitDishDatabaseContext _dbContext;
        public AddressController(SwitDishDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Route("Address/GetBrandings")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetBrandings()
        {
            var list = new List<Branding>();
            try
            {
                list = _dbContext.Brandings.ToList();
                //var mylist = _dbContext.Brandings.ToList();
                //var Colourlist = _dbContext.Brandings.ToList();
                return this.Ok(list);
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

        [Route("Address/SaveOrUpdateBranding")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveOrUpdateBranding(Branding branding)
        {
            try
            {
                Branding obj = _dbContext.Brandings.Where(a => a.ControlName == branding.ControlName).FirstOrDefault();
                if (obj != null)
                {
                    obj.Color = branding.Color;
                    _dbContext.Entry(obj).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                }
                return Ok();

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

        [Route("Address/UpdateProfile")]
        [HttpPost]
        [AllowAnonymous]
        //[Route("api/agent/submit")]
        public async Task<IActionResult> UpdateProfile(User user)
        {
            try
            {
                User obj = _dbContext.Users.Where(a => a.UserId == user.UserId).FirstOrDefault();
                if (obj != null)
                {
                    obj.FirstName = user.FirstName;
                    obj.LastName = user.LastName;
                    obj.Email = user.Email;
                    obj.Phone = user.Phone;
                    obj.Password = user.Password;
                    _dbContext.Entry(obj).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    return Ok();
                }
                return Ok();

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


        [Route("Address/UpdateAccount")]
        [HttpPost]
        [AllowAnonymous]
        //[Route("api/agent/submit")]
        public async Task<IActionResult> UpdateAccount(BankAccount account)
        {
            try
            {
                BankAccount obj = _dbContext.BankAccounts.Where(a => a.AccountId == account.AccountId).FirstOrDefault();
                BankModel data = new BankModel();
                if (obj != null)
                {
                    obj.BankName = account.BankName;
                    obj.AccountName = account.AccountName;
                    obj.SortCode = account.SortCode;
                    obj.AccountNumber = account.AccountNumber;
                    obj.VendorId = account.VendorId;
                    obj.UserId = account.UserId;


                    data.AccountId = Convert.ToUInt32(account.AccountId);
                    data.BankName = account.BankName;
                    data.AccountName = account.AccountName;
                    data.SortCode = account.SortCode;
                    data.AccountNumber = account.AccountNumber;
                    _dbContext.Entry(obj).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                }
                else
                {
                    account.CreatedDate = DateTime.Now;
                    _dbContext.BankAccounts.Add(account);
                    _dbContext.SaveChanges();
                    data.AccountId = Convert.ToUInt32(account.AccountId);
                    data.BankName = account.BankName;
                    data.AccountName = account.AccountName;
                    data.SortCode = account.SortCode;
                    data.AccountNumber = account.AccountNumber;
                }
                return Ok(data);

            }
            catch (Exception ex)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(ex.Message)
                };
                return this.Ok();
            }
        }


        [Route("Address/GetAddress")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAddress()
        {
            var list = new List<Address>();
            try
            {
                list = _dbContext.Addresses.ToList();
                List<AddressVM> listVM = new List<AddressVM>();
                foreach (var item in list)
                {
                    AddressVM obj = new AddressVM();
                    obj.AddressId = Convert.ToUInt32(item.AddressId);
                    obj.FlatNumber = item.FlatNumber;
                    obj.BuildingNumber = item.BuildingNumber;
                    obj.AddressLine1 = item.AddressLine1;
                    obj.AddressLine2 = item.AddressLine2;
                    obj.PostCode = item.PostCode;
                    obj.County = item.County;
                    obj.Country = item.Country;
                    obj.Active = item.Active;
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
    public class BankModel
    {
        public long AccountId { get; set; }
        public string BankName { get; set; }
        public string AccountName { get; set; }
        public string SortCode { get; set; }
        public string AccountNumber { get; set; }
        public Nullable<int> VendorId { get; set; }
        public Nullable<int> BranchId { get; set; }
        public System.DateTime CreatedDate { get; set; }
    }
}
