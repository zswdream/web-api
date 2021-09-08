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
    public class VendorController : ControllerBase
    {
        private SwitDishDatabaseContext _dbContext;
        public VendorController(SwitDishDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Route("Vendors/GetVendors")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetVendors()
        {
            var list = new List<DataModel_OLD.Models.Vendor>();
            try
            {
                list = _dbContext.Vendors.ToList();
                List<VendorVM> listVM = new List<VendorVM>();
                foreach (var item in list)
                {
                    VendorVM obj = new VendorVM();
                    obj.VendorId = Convert.ToUInt32(item.VendorId);
                    obj.Name = item.Name;
                    obj.VendorDate = item.Date?.ToString("M/d/yyyy");
                    obj.IsVendorDeliver = item.IsVendorDeliver;
                    obj.AddressId = Convert.ToUInt32(item.AddressId);
                    obj.Address = "Address 1";
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


        [Route("Vendors/GetVendorsAccounts")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetVendorsAccounts()
        {
            try
            {
               var list = _dbContext.BankAccounts.Where(a=>a.VendorId != null).ToList();
                List<BankAccountVM> listVM = new List<BankAccountVM>();
                foreach (var item in list)
                {
                    BankAccountVM obj = new BankAccountVM();
                    obj.VendorId = Convert.ToInt32(item.VendorId);
                    if (item.VendorId != null)
                    {
                        int id = int.Parse(item.VendorId);
                        obj.VendorName = _dbContext.Vendors.Where(a => a.VendorId == id).FirstOrDefault().Name;
                    }
                    obj.AccountId = item.AccountId;
                    obj.AccountName = item.AccountName;
                    obj.BankName = item.BankName;
                    obj.SortCode = item.SortCode;
                    obj.BanckAccountDate = item.CreatedDate.ToString("M/d/yyyy");
                    obj.AccountNumber = item.AccountNumber;
                    obj.BranchId = item.BranchId;
                    if (item.BranchId != null)
                    {
                        obj.BranchName = _dbContext.Branches.Where(a => a.BranchId == item.BranchId).FirstOrDefault().BranchName;
                    }
                    obj.BanckAccountDate = item.CreatedDate.ToString("M/d/yyyy");
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


        [Route("Vendors/SaveOrUpdate")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveOrUpdate(DataModel_OLD.Models.Vendor vendor)
        {
            try
            {
                var list = new List<DataModel_OLD.Models.Vendor>();
                if (Convert.ToUInt32(vendor.VendorId) > 0)
                {
                    vendor.Date = DateTime.Now;
                    _dbContext.Entry(vendor).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    list = _dbContext.Vendors.ToList();
                    List<VendorVM> listVM = new List<VendorVM>();
                    foreach (var item in list)
                    {
                        VendorVM obj = new VendorVM();
                        obj.Name = item.Name;
                        //"current Account" ? "Saving Account" : User.Identity.Name;
                        obj.VendorDate = item.Date?.ToString("M/d/yyyy"); 
                        obj.AddressId = Convert.ToUInt32(item.AddressId);

                        listVM.Add(obj);
                    }
                    return this.Ok(listVM);

                }
                else
                {
                    vendor.Date = DateTime.Now;
                    _dbContext.Vendors.Add(vendor);
                    _dbContext.SaveChanges();
                    list = _dbContext.Vendors.ToList();
                    List<VendorVM> listVM = new List<VendorVM>();
                    foreach (var item in list)
                    {
                        VendorVM obj = new VendorVM();
                        obj.Name = item.Name;
                        obj.VendorDate = item.Date?.ToString("M/d/yyyy");
                        obj.AddressId = Convert.ToUInt32(item.AddressId);

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


        [Route("Vendors/SaveOrUpdateVendorAccount")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveOrUpdateVendorAccount(BankAccount vendorAccount)
        {
            try
            {
                var list = new List<DataModel_OLD.Models.Vendor>();
                if (Convert.ToUInt32(vendorAccount.AccountId) > 0)
                {
                    vendorAccount.CreatedDate = DateTime.Now;
                    _dbContext.Entry(vendorAccount).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    return this.Ok();
                }
                else
                {
                    vendorAccount.CreatedDate = DateTime.Now;
                    _dbContext.BankAccounts.Add(vendorAccount);
                    _dbContext.SaveChanges();
                    return this.Ok();
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


        [Route("Vendors/DeleteById")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteById(DataModel_OLD.Models.Vendor vendor)
        {
            try
            {
                var list = new List<DataModel_OLD.Models.Vendor>();
                vendor = _dbContext.Vendors.Where(a => a.VendorId == vendor.VendorId).FirstOrDefault();
                _dbContext.Vendors.Remove(vendor);
                _dbContext.SaveChanges();
                list = _dbContext.Vendors.ToList();
                List<VendorVM> listVM = new List<VendorVM>();
                foreach (var item in list)
                {
                    VendorVM obj = new VendorVM();
                    obj.Name = item.Name;
                    obj.VendorDate = item.Date?.ToString("M/d/yyyy");
                    obj.AddressId = Convert.ToUInt32(item.AddressId);

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


        [Route("VendorAccount/BanckAccountDeleteById")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> BanckAccountDeleteById(long AccountId)
        {
            try
            {
                var list = new List<BankAccountVM>();
                var data = _dbContext.BankAccounts.Where(a => a.AccountId == AccountId).FirstOrDefault();
                _dbContext.BankAccounts.Remove(data);
                _dbContext.SaveChanges();
                 
                    var mydata = _dbContext.BankAccounts.ToList();
                    List<BankAccountVM> listVM = new List<BankAccountVM>();
                foreach (var item in mydata)
                {
                    BankAccountVM obj = new BankAccountVM();
                    obj.VendorId = Convert.ToInt32(item.VendorId);
                    if (item.VendorId != null)
                    {
                        int id = int.Parse(item.VendorId);
                        obj.VendorName = _dbContext.Vendors.Where(a => a.VendorId == id).FirstOrDefault().Name;
                    }
                    obj.AccountId = item.AccountId;
                    obj.AccountName = item.AccountName;
                    obj.BankName = item.BankName;
                    obj.SortCode = item.SortCode;
                    obj.BanckAccountDate = item.CreatedDate.ToString("M/d/yyyy");
                    obj.AccountNumber = item.AccountNumber;
                    obj.BranchId = item.BranchId;
                    if (item.BranchId != null)
                    {
                        obj.BranchName = _dbContext.Branches.Where(a => a.BranchId == item.BranchId).FirstOrDefault().BranchName;
                    }
                    obj.BanckAccountDate = item.CreatedDate.ToString("M/d/yyyy");
                    listVM.Add(obj);

                    
                }
                return Ok(listVM);
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