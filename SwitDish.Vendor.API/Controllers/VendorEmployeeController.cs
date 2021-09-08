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
    public class VendorEmployeeController : ControllerBase
    {
        private SwitDishDatabaseContext _dbContext;
        public VendorEmployeeController(SwitDishDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Route("VendorEmployee/GetVendorEmployee")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetVendorEmployee()
        {
            var list = new List<VendorEmployee>();
            try
            {
                list = _dbContext.VendorEmployees.ToList();
                List<VendorEmployeeVM> listVM = new List<VendorEmployeeVM>();
                foreach (var item in list)
                {
                    VendorEmployeeVM obj = new VendorEmployeeVM();
                    obj.VendorEmployeeId = item.VendorEmployeeId;
                    obj.UserId = Convert.ToUInt32(item.Vendor.UserId);
                    obj.BranchId = item.BranchId;
                    obj.VendorId = Convert.ToUInt32(item.VendorId);
                    var name = _dbContext.Users.Where(x => x.RoleId == 1).ToList().FirstOrDefault();
                    obj.Name = string.Empty;
                    if (name != null)
                    {
                        obj.Name = name.FirstName;
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
