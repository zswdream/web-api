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
    public class AccumulationRuleController : ControllerBase
    {
        private SwitDishDatabaseContext _dbContext;
        public AccumulationRuleController(SwitDishDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Route("Rules/GetRules")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetRules()
        {
            var list = new List<AccumulationRule>();
            try
            {
                list = _dbContext.AccumulationRules.ToList();
                List<RuleVM> listVM = new List<RuleVM>();
                foreach (var item in list)
                {
                    RuleVM obj = new RuleVM();
                    obj.RuleId = item.RuleId;
                    obj.Name = item.Name;
                    obj.Description = item.Description;
                    obj.DateCreated = item.DateCreated.ToString("M/d/yyyy");
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


        [Route("Rules/SaveOrUpdate")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveOrUpdate(AccumulationRule rule)
        {
            try
            {
                var list = new List<AccumulationRule>();
                if (rule.RuleId > 0)
                {
                    rule.DateCreated = DateTime.Now;
                    _dbContext.Entry(rule).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    list = _dbContext.AccumulationRules.ToList();
                    List<RuleVM> listVM = new List<RuleVM>();
                    foreach (var item in list)
                    {
                        RuleVM obj = new RuleVM();
                        obj.RuleId = item.RuleId;
                        obj.Name = item.Name;
                        obj.Description = item.Description;
                        obj.DateCreated = item.DateCreated.ToString("M/d/yyyy");
                        listVM.Add(obj);
                    }
                    return this.Ok(listVM);

                }
                else
                {
                    rule.DateCreated = DateTime.Now;
                    _dbContext.AccumulationRules.Add(rule);
                    _dbContext.SaveChanges();
                    list = _dbContext.AccumulationRules.ToList();
                    List<RuleVM> listVM = new List<RuleVM>();
                    foreach (var item in list)
                    {
                        RuleVM obj = new RuleVM();
                        obj.RuleId = item.RuleId;
                        obj.Name = item.Name;
                        obj.Description = item.Description;
                        obj.DateCreated = item.DateCreated.ToString("M/d/yyyy");
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

        [Route("Rules/DeleteById")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteById(AccumulationRule rule)
        {
            try
            {
                var list = new List<AccumulationRule>();
                rule = _dbContext.AccumulationRules.Where(a => a.RuleId == rule.RuleId).FirstOrDefault();
                _dbContext.AccumulationRules.Remove(rule);
                _dbContext.SaveChanges();
                list = _dbContext.AccumulationRules.ToList();
                List<RuleVM> listVM = new List<RuleVM>();
                foreach (var item in list)
                {
                    RuleVM obj = new RuleVM();
                    obj.RuleId = item.RuleId;
                    obj.Name = item.Name;
                    obj.Description = item.Description;
                    obj.DateCreated = item.DateCreated.ToString("M/d/yyyy");
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
