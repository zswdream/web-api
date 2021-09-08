using AutoMapper;
using DataModel;
using SwitDish.Domain.Contract;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Linq;
using SwitDish.Domain.Models;

namespace SwitDish.Domain.Services
{
    public class AuthService
    {
        private SwitDishDatabaseEntities _dbContext;
        public AuthService(SwitDishDatabaseEntities dbContext)
        {
            _dbContext = dbContext;
        }


        /// <summary>
        ///  Login
        /// </summary>
        /// <returns></returns>
        public async Task<DataModel.User> Login(DataModel.User user)
        {
            var result = new DataModel.User();
            try
            {

            }
            catch (Exception)
            {
                //TODO: Handle expection to log into app insights
                throw;
            }
            return result;
        }

        public async Task<DataModel.User> Register(DataModel.User user)
        {
            var result = _dbContext.Users.Where(a => a.Email == user.Email).FirstOrDefault();
            try
            {
                // if user is not already exist
            if (result == null)
            {
                    var newUser =new DataModel.User();
                    newUser = user;
                    _dbContext.Users.Add(newUser);
                    _dbContext.SaveChanges();
                    return newUser;
            }
                // if user already exist
            else
            {
                    return result;
            }
            }
            catch (Exception ex)
            {
                return result;
            }
        }

    }
}
