using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SwitDish.DataModel_OLD.Models;
using SwitDish.Vendor.API.Models;
using Microsoft.Extensions.Configuration;

namespace SwitDish.Vendor.API.Controllers
{
    public class AuthController : ControllerBase
    {
        private readonly SwitDishDatabaseContext _dbContext;
        private readonly IConfiguration config;
        public AuthController(SwitDishDatabaseContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            this.config = config;
        }
        [Route("Auth/Login")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(DataModel_OLD.Models.User user)
        {
            try
            {
            if (user.AuthMethod == "Facebook")
            {
                var Fbuser = _dbContext.Users.Where(a => a.Email == user.Email && a.AuthMethod == "Facebook").FirstOrDefault();
                if (Fbuser == null)
                {
                        var newUser = new DataModel_OLD.Models.User();
                        newUser.Email = user.Email;
                        newUser.Password = "12345";
                        newUser.FirstName = user.FirstName;
                        newUser.LastName = user.LastName;
                        newUser.Username = user.Email;
                        newUser.AuthMethod = user.AuthMethod;
                        newUser.AccessToken = user.AccessToken;
                        newUser.Active = true;
                        newUser.Phone = "TBD";
                        newUser.Title = "Title";
                        newUser.RoleId = 4;
                        _dbContext.Users.Add(newUser);
                        _dbContext.SaveChanges();
                        return this.Ok(newUser);
                    }
                // if user exist
                else
                {
                    return this.Ok(Fbuser);
                }
            }
            else if (user.AuthMethod == "Google")
            {
                var Googleuser = _dbContext.Users.Where(a => a.Email == user.Email && a.AuthMethod == "Google").FirstOrDefault();
                if (Googleuser == null)
                {
                        var newUser = new DataModel_OLD.Models.User();
                        newUser.Email = user.Email;
                        newUser.Password = "12345";
                        newUser.FirstName = user.FirstName;
                        newUser.LastName = user.LastName;
                        newUser.Username = user.Email;
                        newUser.AuthMethod = user.AuthMethod;
                        newUser.AccessToken = user.AccessToken;
                        newUser.Active = true;
                        newUser.Phone = "TBD";
                        newUser.Title = "Title";
                        newUser.RoleId = 4;
                        _dbContext.Users.Add(newUser);
                        _dbContext.SaveChanges();
                        return this.Ok(newUser);
                    }
                // if user exist
                else
                {
                    return this.Ok(Googleuser);
                }
            }
            else
            {
                var result = _dbContext.Users.Where(a => a.Email == user.Email && a.Password == user.Password).FirstOrDefault();
                    // if user is not exist
                   User response = new User();
                if (result == null)
                {
                    return this.Ok("Incorect Username or Password");
                }
                // if user exist
                else
                {
                        response.UserId = result.UserId;
                        response.RoleId = result.RoleId;
                        response.Email = result.Email;
                        response.Username = result.Username;
                        response.FirstName = result.FirstName;
                        response.Phone = result.Phone;
                        response.Rating = result.Rating;
                        response.Gender = result.Gender;
                        return this.Ok(response);
                }
            }
            }
            catch (Exception ex)
            {
                return this.Ok(ex.Message);
            }
        }

        [Route("Auth/Register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] DataModel_OLD.Models.User user)
        {
           try
            {
                var result = _dbContext.Users.Where(a => a.Email == user.Email).FirstOrDefault();
                // if user is not already exist
                if (result == null)
                {
                    var newUser = new DataModel_OLD.Models.User();
                    newUser = user;
                    newUser.Email = user.Email;
                    newUser.Password = user.Password;
                    newUser.FirstName = user.FirstName;
                    newUser.LastName = user.LastName;
                    newUser.Username = user.Email;
                    newUser.Active = true;
                    newUser.Phone = user.Phone;
                    newUser.Title = "Title";
                    newUser.RoleId = 4;
                    newUser.UserDate = DateTime.Now;
                    _dbContext.Users.Add(newUser);
                    _dbContext.SaveChanges();
                    return this.Ok(newUser);
                }
                // if user already exist
                else
                {
                    return this.Ok("Already Exist");
                }
            }
            catch (Exception ex)
            {
                return this.Ok("Fail");
            }
        }
        [Route("Auth/ForgotPassword")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult>ForgotPassword(DataModel_OLD.Models.User user)
        {
            Guid obj = Guid.NewGuid();
            var myvalue = obj.ToString().Substring(0, 5);
            try
            {
                
                var User = _dbContext.Users.Where(a => a.Email == user.Email).FirstOrDefault();
                List<User> Mylist = new List<User>();
                User.Password = myvalue;
                _dbContext.Entry(User).State = EntityState.Modified;
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                 return Ok("failed");
            }
            var myEmail = this.config.GetValue<string>("EmailSettings:Email");
            var myPassword = this.config.GetValue<string>("EmailSettings:Password");

            using (MailMessage mm = new MailMessage(myEmail, user.Email))
            {
                //mm.From = new MailAddress(myEmail, "SenderID");
                //mm.To.Add(new MailAddress(user.Email, "ReceiverID"));

                mm.Subject = "Updated Password";
                mm.Body = "Your Updated Password Is" + myvalue;
                mm.IsBodyHtml = false;
                //mm.CC.Add(new MailAddress("smartboy1994@gmail.com"));


                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential(myEmail, myPassword);
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = 587;
                    smtp.Send(mm);
                }
            }
            return Ok("success");
        }
        
        }
    
}