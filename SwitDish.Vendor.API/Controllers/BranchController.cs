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
    public class BranchController : ControllerBase
    {
        private SwitDishDatabaseContext _dbContext;
        public BranchController(SwitDishDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Route("Branches/GetBranches")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetBranches()
        {
            var list = new List<Branch>();
            try
            {
                list = _dbContext.Branches.ToList();
                List<BranchVM> listVM = new List<BranchVM>();
                foreach (var item in list)
                {
                    BranchVM obj = new BranchVM();
                    obj.BranchId = item.BranchId;
                    obj.BranchName = item.BranchName;
                    obj.IsHeadQuarters = item.IsHeadQuarters;
                    obj.AddressBrance = item.Address;
                    //var address = _dbContext.Addresses.FirstOrDefault();
                    //obj.Address = string.Empty;
                    //if (address != null)
                    //{
                    //    obj.Address = address.AddressLine1;
                    //}
                    obj.Address = item.Address;
                    obj.Manager = item.Manager;
                    obj.Vendor = item.Vendor;
                    //var vendor = _dbContext.Vendors.Where(x => x.VendorId == obj.VendorId).ToList().FirstOrDefault();
                    //obj.VendorName = string.Empty;
                    //if (vendor != null)
                    //{
                    //    obj.VendorName = vendor.Name;
                    //}
                    obj.Account = item.Account;
                    var account = _dbContext.BankAccounts.ToList();
                    obj.Account_Name = string.Empty;
                    if (account != null)
                    {
                        
                        foreach (var it in account)
                        {
                            obj.Account_Name = it.AccountName;
                            obj.Account_Type = "Saving Account";
                            obj.Account_Number = it.AccountNumber;
                            //obj.UserId = (int)it.UserID;
                        }
                        
                    }
                    obj.Account = item.Account;
                    obj.Manager = item.Manager;
                    //var manager = _dbContext.Users.Where(x => x.UserId== obj.ManagerId).ToList().FirstOrDefault();
                    //obj.ManagerName = string.Empty;
                    //if (manager != null)
                    //{
                    //    obj.ManagerName = manager.FirstName;
                    //}
                  
                    obj.Vendor = item.Vendor;
                    obj.Account = item.Account;
                    obj.DateCreate = item.DateCreated.ToString("M/d/yyyy"); 
                    obj.IsActive = item.IsActive;
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
        
   [Route("Branches/UpdateBranchEmployees")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateBranchEmployees(BranchEmployeeVM branch)
        {
            try
            {
                if (branch.selected == false)
                {
                    BranchEmployee obj = new BranchEmployee();
                    obj.EmployeeId =branch.EmployeeId;
                    obj.BranchId = branch.BranchId;
                    _dbContext.BranchEmployees.Add(obj);
                    _dbContext.SaveChanges();
                }
                else
                {
                    var employee = _dbContext.BranchEmployees.Where(a=>a.BranchEmployeeId == branch.BranchEmployeeId).FirstOrDefault();
                    _dbContext.BranchEmployees.Remove(employee);
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


        [Route("Branches/SaveOrUpdate")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveOrUpdate(Branch branch)
        {
            try
            {
                var list = new List<Branch>();
                if (branch.BranchId > 0)
                {
                    branch.DateCreated = DateTime.Now;
                    _dbContext.Entry(branch).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    list = _dbContext.Branches.ToList();
                    List<BranchVM> listVM = new List<BranchVM>();
                    foreach (var item in list)
                    {
                        BranchVM obj = new BranchVM();
                        obj.BranchId = item.BranchId;
                        obj.BranchName = item.BranchName;
                        obj.IsHeadQuarters = item.IsHeadQuarters;
                        obj.Address = item.Address;
                        obj.Manager = item.Manager;
                        obj.Vendor = item.Vendor;
                        obj.Account = item.Account;
                        obj.DateCreate = item.DateCreated.ToString("M/d/yyyy");
                        obj.IsActive = item.IsActive;
                        listVM.Add(obj);
                    }
                    return this.Ok(listVM);

                }
                else
                {
                    branch.DateCreated = DateTime.Now;
                    branch.Vendor = "Umar";
                    _dbContext.Branches.Add(branch);
                    _dbContext.SaveChanges();
                    list = _dbContext.Branches.ToList();
                    List<BranchVM> listVM = new List<BranchVM>();
                    foreach (var item in list)
                    {
                        BranchVM obj = new BranchVM();
                        obj.BranchId = item.BranchId;
                        obj.BranchName = item.BranchName;
                        obj.IsHeadQuarters = item.IsHeadQuarters;
                        obj.Address = item.Address;
                        obj.Manager = item.Manager;
                        obj.Vendor = item.Vendor;
                        obj.Account = item.Account;
                        obj.DateCreate = item.DateCreated.ToString("M/d/yyyy");
                        obj.IsActive = item.IsActive;
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

        [Route("Branches/DeleteById")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteById(Branch branch)
        {
            try
            {
                var list = new List<Branch>();
                branch = _dbContext.Branches.Where(a => a.BranchId == branch.BranchId).FirstOrDefault();
                var emp = _dbContext.BranchEmployees.Where(a => a.BranchId == branch.BranchId).ToList();
                foreach (var item in emp)
                {
                    _dbContext.BranchEmployees.Remove(item);
                    _dbContext.SaveChanges();
                }
                _dbContext.Branches.Remove(branch);
                _dbContext.SaveChanges();
                list = _dbContext.Branches.ToList();
                List<BranchVM> listVM = new List<BranchVM>();
                foreach (var item in list)
                {
                    BranchVM obj = new BranchVM();
                    obj.BranchId = item.BranchId;
                    obj.BranchName = item.BranchName;
                    obj.IsHeadQuarters = item.IsHeadQuarters;
                    obj.Address = item.Address;
                    obj.Manager = item.Manager;
                    obj.Vendor = item.Vendor;
                    obj.Account = item.Account;
                    obj.DateCreate = item.DateCreated.ToString("M/d/yyyy");
                    obj.IsActive = item.IsActive;
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

        [Route("Branches/GetBranchEmployee")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetBranchEmployee()
        {
            var list = new List<BranchEmployee>();
            try
            {
                var httpRequest = HttpContext.Request;
                var branchId = Convert.ToInt32(httpRequest.Query["branchEmployeeId"]);
                var users = _dbContext.Users.Where(a=>a.RoleId==2).ToList();
                List<BranchEmployeeVM> listVM = new List<BranchEmployeeVM>();
                foreach (var item in users)
                {
                    BranchEmployeeVM obj = new BranchEmployeeVM();
                    var mydata = item.UserId;
                    var branchEmployee = _dbContext.BranchEmployees.Where(a=>a.EmployeeId== mydata).FirstOrDefault();
                    if (branchEmployee != null)
                    {
                    obj.BranchEmployeeId = branchEmployee.BranchEmployeeId;
                    obj.BranchId = branchEmployee.BranchId;
                    obj.EmployeeId = Convert.ToUInt32(item.UserId);
                    obj.EmployeeName = item.FirstName;
                    obj.EmployeeEmail = item.Email;
                    obj.EmployeeRole = "";
                    obj.selected = true;
                    }
                    else
                    {
                        obj.BranchEmployeeId = 0;
                        obj.BranchId = 0;
                        obj.EmployeeId = Convert.ToUInt32(item.UserId);
                        obj.EmployeeName = item.FirstName;
                        obj.EmployeeEmail = item.Email;
                        obj.EmployeeRole = "";
                        obj.selected = false;
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
        [Route("Branches/GetManager")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetManager()
        {
            var list = new List<User>();
            try
            {
                list = _dbContext.Users.Where(x=>x.RoleId == 1).ToList();
                List<UserVM> listVM = new List<UserVM>();
                foreach (var item in list)
                {
                    UserVM obj = new UserVM();
                    obj.UserId = Convert.ToUInt32(item.UserId);
                    obj.FirstName = item.FirstName;
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