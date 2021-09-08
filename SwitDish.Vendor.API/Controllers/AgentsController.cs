// Template: Controller Implementation (ApiControllerImplementation.t4) version 3.0

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
    public class AgentsController : ControllerBase
    {
        private SwitDishDatabaseContext _dbContext;
        public AgentsController(SwitDishDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Route("Agents/GetAgents")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAgents()
        {
            var list = new List<User>();
            try
            {
                list = _dbContext.Users.Where(a=>a.RoleId==3).ToList();
                List<AgentVM> listVM = new List<AgentVM>();
                foreach (var item in list)
                {
                    var user = _dbContext.Users.Where(a=>a.UserId==item.UserId).FirstOrDefault();
                    AgentVM obj = new AgentVM();
                    obj.AgentId = int.Parse(item.UserId.ToString());
                    obj.FirstName = user.FirstName;
                    obj.LastName = user.LastName;
                    obj.Username = user.Username;
                    obj.Email = user.Email;
                    obj.Active = user.Active;
                    obj.Phone = user.Phone;
                    obj.Gender = user.Gender;
                    obj.AccountId = Convert.ToUInt32(user.AccountId);
                    obj.CreatedDates = DateTime.Now;
                    obj.Role = "Agent";
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

        [Route("Agents/UpdateVendorAgents")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> UpdateVendorAgents(AgentVM agent)
        {
            try
            {
                if (agent.Selected == false)
                {
                    VendorAgent obj = new VendorAgent();
                    obj.VendorId =agent.VendorId;
                    obj.AgentId = agent.AgentId;
                     obj.Active = true;
                    _dbContext.VendorAgents.Add(obj);
                    _dbContext.SaveChanges();
                }
                else
                {
                    var employee = _dbContext.VendorAgents.Where(a => a.VendorAgentId == agent.VendorAgentId).FirstOrDefault();
                    _dbContext.VendorAgents.Remove(employee);
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



        [Route("Agents/VendorAgents")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> VendorAgents(DataModel_OLD.Models.Vendor vendor)
        {
            var list = new List<User>();
            try
            {
                list = _dbContext.Users.Where(a=>a.RoleId==3).ToList();
                List<AgentVM> listVM = new List<AgentVM>();
                foreach (var item in list)
                {
                    var vendorAgent = _dbContext.VendorAgents.Where(a => a.AgentId == Convert.ToUInt32(item.UserId) && a.VendorId== vendor.VendorId).FirstOrDefault();
                    AgentVM obj = new AgentVM();
                    obj.AgentId = int.Parse(item.UserId.ToString());
                    obj.FirstName = item.FirstName;
                    obj.LastName = item.LastName;
                    obj.Username = item.Username;
                    obj.Email = item.Email;
                    obj.Active = item.Active;
                    obj.Phone = item.Phone;
                    obj.Gender = item.Gender;
                    obj.AccountId = Convert.ToUInt32(item.AccountId);
                    obj.CreatedDates = DateTime.Now;
                    obj.Role = "Agent";
                    if (vendorAgent != null)
                    {
                        obj.VendorAgentId = Convert.ToInt32(vendorAgent.VendorAgentId);
                        obj.Selected = true;
                    }
                    else
                    {
                        obj.VendorAgentId = 0;
                        obj.Selected = false;
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



        [Route("Agents/SaveOrUpdate")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveOrUpdate(AgentVM agent)
        {
            try
            {
                var list = new List<Agent>();
                Agent Agentobj = new Agent();
                Agentobj.UserId = agent.UserId;
                Agentobj.CompanyId = agent.CompanyId;
                if (agent.AgentId > 0)
                {
                    _dbContext.Entry(Agentobj).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    list = _dbContext.Agents.ToList();
                    List<AgentVM> listVM = new List<AgentVM>();
                    foreach (var item in list)
                    {
                         
                        var user = _dbContext.Users.Where(a => a.UserId == item.UserId).FirstOrDefault();
                        AgentVM obj = new AgentVM();
                        obj.AgentId = int.Parse(item.AgentId.ToString());
                        obj.FirstName = user.FirstName;
                        obj.LastName = user.LastName;
                        obj.Username = user.Username;
                        obj.Email = user.Email;
                        obj.Active = user.Active;
                        obj.Phone = user.Phone;
                        obj.Gender = user.Gender;
                        obj.AccountId = Convert.ToUInt32(user.AccountId);
                        listVM.Add(obj);
                    }
                    return this.Ok(listVM);

                }
                else
                {
                    _dbContext.Agents.Add(Agentobj);
                    _dbContext.SaveChanges();
                    list = _dbContext.Agents.ToList();
                    List<AgentVM> listVM = new List<AgentVM>();
                    foreach (var item in list)
                    {
                         
                        var user = _dbContext.Users.Where(a => a.UserId == item.UserId).FirstOrDefault();
                        AgentVM obj = new AgentVM();
                        obj.AgentId = int.Parse(item.AgentId.ToString());
                        obj.FirstName = user.FirstName;
                        obj.LastName = user.LastName;
                        obj.Username = user.Username;
                        obj.Email = user.Email;
                        obj.Active = user.Active;
                        obj.Phone = user.Phone;
                        obj.Gender = user.Gender;
                        obj.AccountId = Convert.ToUInt32(user.AccountId);
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

        [Route("Agents/DeleteById")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteById(AgentVM agent)
        {
            try
            {
                Agent agentDelete  = _dbContext.Agents.Where(a => a.AgentId == agent.AgentId).FirstOrDefault();
                _dbContext.Agents.Remove(agentDelete);
                _dbContext.SaveChanges();
              var  list = _dbContext.Agents.ToList();
                List<AgentVM> listVM = new List<AgentVM>();
                foreach (var item in list)
                {
                    
                    var user = _dbContext.Users.Where(a => a.UserId == item.UserId).FirstOrDefault();
                    AgentVM obj = new AgentVM();
                    obj.AgentId = int.Parse(item.AgentId.ToString());
                    obj.FirstName = user.FirstName;
                    obj.LastName = user.LastName;
                    obj.Username = user.Username;
                    obj.Email = user.Email;
                    obj.Active = user.Active;
                    obj.Phone = user.Phone;
                    obj.Gender = user.Gender;
                    obj.AccountId = Convert.ToUInt32(user.AccountId);
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
