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
    public class UsersController : ControllerBase
    {
        private SwitDishDatabaseContext _dbContext;
        public UsersController(SwitDishDatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        [Route("Users/GetEmployees")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetEmployees()
        {
            var list = new List<User>();
            try
            {
                list = _dbContext.Users.Where(a=>a.RoleId==4).ToList();
                List<UserVM> listVM = new List<UserVM>();
                foreach (var item in list)
                {
                    UserVM obj = new UserVM();
                    obj.UserId = Convert.ToUInt32(item.UserId);
                    obj.FirstName = item.FirstName;
                    obj.LastName = item.LastName;
                    obj.Username = item.Username;
                    obj.Password = item.Password;
                    obj.Email = item.Email;
                    obj.Active = item.Active;
                    obj.Phone = item.Phone;
                    obj.Gender = item.Gender;
                    obj.AccountId = Convert.ToUInt32(item.AccountId);
                    obj.UserDate = item.UserDate.ToString("M/d/yyyy");
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


        [Route("Users/GetAgents")]
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAgents()
        {
            var list = new List<User>();
            try
            {
                list = _dbContext.Users.Where(a => a.RoleId == 3).ToList();
                List<UserVM> listVM = new List<UserVM>();
                foreach (var item in list)
                {
                    UserVM obj = new UserVM();
                    obj.UserId = Convert.ToUInt32(item.UserId);
                    obj.FirstName = item.FirstName;
                    obj.LastName = item.LastName;
                    obj.Username = item.Username;
                    obj.Password = item.Password;
                    obj.Email = item.Email;
                    obj.Active = item.Active;
                    obj.Phone = item.Phone;
                    obj.Gender = item.Gender;
                    obj.AccountId = Convert.ToUInt32(item.AccountId);
                    obj.rating = item.Rating;
                    obj.UserDate = item.UserDate.ToString("M/d/yyyy");
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

        [Route("Users/SaveOrUpdateAgent")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SaveOrUpdateAgent(User user)
        {
            try
            {
                var list = new List<User>();
                user.RoleId = 3;
                user.Username = user.FirstName + " " + user.LastName;
                user.Password = "12345";
                if (Convert.ToUInt32(user.UserId) > 0)
                {
                    _dbContext.Entry(user).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    list = _dbContext.Users.Where(a=>a.RoleId==3).ToList();
                    List<UserVM> listVM = new List<UserVM>();
                    foreach (var item in list)
                    {
                        UserVM obj = new UserVM();
                        obj.UserId = Convert.ToUInt32(item.UserId);
                        obj.FirstName = item.FirstName;
                        obj.LastName = item.LastName;
                        obj.Username = item.Username;
                        obj.Password = item.Password;
                        obj.Email = item.Email;
                        obj.Active = item.Active;
                        obj.Phone = item.Phone;
                        obj.Gender = item.Gender;
                        obj.AccountId = Convert.ToUInt32(item.AccountId);
                        obj.authMethod = item.AuthMethod;
                        obj.accessToken = item.AccessToken;
                        obj.authToken = item.AuthToken;
                        obj.UserDate = item.UserDate.ToString("M/d/yyyy");
                        obj.idToken = item.IdToken;
                        listVM.Add(obj);
                    }
                    return this.Ok(listVM);

                }
                else
                {
                    user.UserDate =Convert.ToDateTime(DateTime.Now.ToString());
                    _dbContext.Users.Add(user);
                    _dbContext.SaveChanges();
                    list = _dbContext.Users.Where(a => a.RoleId == 3).ToList();
                    List<UserVM> listVM = new List<UserVM>();
                    foreach (var item in list)
                    {
                        UserVM obj = new UserVM();
                        obj.UserId = Convert.ToUInt32(item.UserId);
                        obj.FirstName = item.FirstName;
                        obj.LastName = item.LastName;
                        obj.Username = item.Username;
                        obj.Password = item.Password;
                        obj.Email = item.Email;
                        obj.Active = item.Active;
                        obj.Phone = item.Phone;
                        obj.Gender = item.Gender;
                        obj.AccountId = Convert.ToUInt32(item.AccountId);
                        obj.authMethod = item.AuthMethod;
                        obj.accessToken = item.AccessToken;
                        obj.authToken = item.AuthToken;
                        obj.idToken = item.IdToken;
                        obj.UserDate = item.UserDate.ToString("M/d/yyyy");
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


        [Route("Users/SaveOrUpdate")]
        [HttpPost]
        [AllowAnonymous]
        //[Route("api/agent/submit")]
        public async Task<IActionResult> SaveOrUpdate(User user)
        {
            try
            {
                var list = new List<User>();
                user.RoleId = 4;
                user.Username = user.FirstName;
                user.Password = "12345";
                if (Convert.ToUInt32(user.UserId) > 0)
                {
                    _dbContext.Entry(user).State = EntityState.Modified;
                    _dbContext.SaveChanges();
                    list = _dbContext.Users.Where(a=>a.RoleId==4).ToList();
                    List<UserVM> listVM = new List<UserVM>();
                    foreach (var item in list)
                    {
                        UserVM obj = new UserVM();
                        obj.UserId = Convert.ToUInt32(item.UserId);
                        obj.FirstName = item.FirstName;
                        obj.LastName = item.LastName;
                        obj.Username = item.Username;
                        obj.Password = item.Password;
                        obj.Email = item.Email;
                        obj.Active = item.Active;
                        obj.Phone = item.Phone;
                        obj.Gender = item.Gender;
                        obj.AccountId = Convert.ToUInt32(item.AccountId);
                        obj.authMethod = item.AuthMethod;
                        obj.accessToken = item.AccessToken;
                        obj.authToken = item.AuthToken;
                        obj.idToken = item.IdToken;
                        obj.UserDate = item.UserDate.ToString("M/d/yyyy");
                        listVM.Add(obj);
                    }
                    return this.Ok(listVM);

                }
                else
                {
                    user.UserDate = Convert.ToDateTime(DateTime.Now.ToString());
                    _dbContext.Users.Add(user);
                    _dbContext.SaveChanges();
                    list = _dbContext.Users.Where(a => a.RoleId == 4).ToList();
                    List<UserVM> listVM = new List<UserVM>();
                    foreach (var item in list)
                    {
                        UserVM obj = new UserVM();
                        obj.UserId = Convert.ToUInt32(item.UserId);
                        obj.FirstName = item.FirstName;
                        obj.LastName = item.LastName;
                        obj.Username = item.Username;
                        obj.Password = item.Password;
                        obj.Email = item.Email;
                        obj.Active = item.Active;
                        obj.Phone = item.Phone;
                        obj.Gender = item.Gender;
                        obj.AccountId = Convert.ToUInt32(item.AccountId);
                        obj.authMethod = item.AuthMethod;
                        obj.accessToken = item.AccessToken;
                        obj.authToken = item.AuthToken;
                        obj.idToken = item.IdToken;
                        obj.UserDate = item.UserDate.ToString("M/d/yyyy");
                        listVM.Add(obj);
                    }
                    return this.Ok(listVM);
                }

            }
            //Exception
            
            catch (Exception ex)
            {
                var response = new HttpResponseMessage(HttpStatusCode.BadRequest)
                {
                    Content = new StringContent(ex.Message)
                };
                return this.Ok(response);
            }
        }

        [Route("Users/DeleteById")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteById(User user)
        {
            try
            {
                var list = new List<User>();
                user = _dbContext.Users.Where(a => a.UserId == user.UserId).FirstOrDefault();
                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();
                list = _dbContext.Users.Where(a => a.RoleId == 4).ToList();
                List<UserVM> listVM = new List<UserVM>();
                foreach (var item in list)
                {
                    UserVM obj = new UserVM();
                    obj.UserId = Convert.ToUInt32(item.UserId);
                    obj.FirstName = item.FirstName;
                    obj.LastName = item.LastName;
                    obj.Username = item.Username;
                    obj.Password = item.Password;
                    obj.Email = item.Email;
                    obj.Active = item.Active;
                    obj.Phone = item.Phone;
                    obj.Gender = item.Gender;
                    obj.AccountId = Convert.ToUInt32(item.AccountId);
                    obj.authMethod = item.AuthMethod;
                    obj.accessToken = item.AccessToken;
                    obj.authToken = item.AuthToken;
                    obj.idToken = item.IdToken;
                    obj.UserDate = item.UserDate.ToString("M/d/yyyy");
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
        [Route("Users/DeleteByIdAgent")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteByIdAgent(User user)
        {
            try
            {
                var list = new List<User>();
                var roleId = user.RoleId;
                user = _dbContext.Users.Where(a => a.UserId == user.UserId).FirstOrDefault();
                _dbContext.Users.Remove(user);
                _dbContext.SaveChanges();
                list = _dbContext.Users.Where(a => a.RoleId == roleId).ToList();
                //list = _dbContext.Users.ToList();
                List<UserVM> listVM = new List<UserVM>();
                foreach (var item in list)
                {
                    UserVM obj = new UserVM();
                    obj.UserId = Convert.ToUInt32(item.UserId);
                    obj.FirstName = item.FirstName;
                    obj.LastName = item.LastName;
                    obj.Username = item.Username;
                    obj.Password = item.Password;
                    obj.Email = item.Email;
                    obj.Active = item.Active;
                    obj.Phone = item.Phone;
                    obj.Gender = item.Gender;
                    obj.AccountId = Convert.ToUInt32(item.AccountId);
                    obj.authMethod = item.AuthMethod;
                    obj.accessToken = item.AccessToken;
                    obj.authToken = item.AuthToken;
                    obj.idToken = item.IdToken;
                    obj.UserDate = item.UserDate.ToString("M/d/yyyy");
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
        [Route("Users/SaveRating")]
        [HttpPost]
        [AllowAnonymous]
        //[Route("api/agent/submit")]
        public async Task<IActionResult> SaveRating(User user)
        {
            try
            {
                var list = new List<User>();
                User agent = _dbContext.Users.Where(a => a.UserId == user.UserId).FirstOrDefault();
                agent.Rating = user.Rating;
                _dbContext.Users.Attach(agent);
                _dbContext.Entry(agent).State = EntityState.Modified;
                _dbContext.SaveChanges();
                list = _dbContext.Users.Where(a => a.RoleId == 3).ToList();
                List<UserVM> listVM = new List<UserVM>();
                foreach (var item in list)
                {
                    UserVM obj = new UserVM();
                    obj.UserId = Convert.ToUInt32(item.UserId);
                    obj.FirstName = item.FirstName;
                    obj.LastName = item.LastName;
                    obj.Username = item.Username;
                    obj.Password = item.Password;
                    obj.Email = item.Email;
                    obj.Active = item.Active;
                    obj.Phone = item.Phone;
                    obj.Gender = item.Gender;
                    obj.AccountId = Convert.ToUInt32(item.AccountId);
                    obj.authMethod = item.AuthMethod;
                    obj.accessToken = item.AccessToken;
                    obj.authToken = item.AuthToken;
                    obj.idToken = item.IdToken;
                    obj.UserDate = item.UserDate.ToString("M/d/yyyy");
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