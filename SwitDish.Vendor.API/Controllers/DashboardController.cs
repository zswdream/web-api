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
    public class DashboardController : ControllerBase
    {  
        private SwitDishDatabaseContext _dbContext;
        public DashboardController(SwitDishDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Route("Dashboard/EmployeeSale")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> EmployeeSale()
        {
            var list = new List<SaleByEmployee>();
            try
            {
                list = _dbContext.SaleByEmployees.ToList();
                List<SaleByEmployeeVM> listVM = new List<SaleByEmployeeVM>();
                foreach (var item in list)
                {
                    SaleByEmployeeVM obj = new SaleByEmployeeVM();
                    obj.SaleByEmployeeId = int.Parse(item.SaleByEmployeeId.ToString());
                    obj.Name = item.Name;
                    obj.TotalSale= item.TotalSale;
                    obj.AverageSale = item.AverageSale;
                    obj.LowestSale = item.LowestSale;
                    obj.HighestSale = item.HighestSale;
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
        [Route("Dashboard/DashboardStats")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> DashboardStats()
        {
            var list = new List<int>();
            try
            {
                // total Managers
                int totalManagers = _dbContext.Users.Where(a => a.RoleId == 2).Count();
                list.Add(totalManagers);
                // total Employees
                int totalEmployees = _dbContext.Users.Where(a => a.RoleId == 4).Count();
                list.Add(totalEmployees);
                // total Agents
                int totalAgents = _dbContext.Users.Where(a => a.RoleId == 3).Count();
                list.Add(totalAgents); 
                // total Sale By Employees
                list.Add(0);
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
        [Route("Dashboard/LocationSale")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> LocationSale()
        {
            var list = new List<SaleByLocation>();
            try
            {
                list = _dbContext.SaleByLocations.ToList();
                List<SaleByLocationVM> listVM = new List<SaleByLocationVM>();
                foreach (var item in list)
                {
                    SaleByLocationVM obj = new SaleByLocationVM();
                    obj.SaleByLocationId = int.Parse(item.SaleByLocationId.ToString());
                    obj.Name = item.Name;
                    obj.TotalSale = item.TotalSale;
                    obj.AverageSale = item.AverageSale;
                    obj.LowestSale = item.LowestSale;
                    obj.HighestSale = item.HighestSale;
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