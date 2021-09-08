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
    public class UserService : ServiceBase, IUserService
    {
        public UserService(SwitDishDatabaseEntities dbContext) : base(dbContext)
        {

        }

        public async Task<Models.User> SaveOrUpdate(Models.User userDetails)
        {
            try
            {
                var dbUser = Mapper.Map<DataModel.User>(userDetails);
                if (userDetails.UserId != 0)
                {
                    // update
                    this.DbContext.Entry(dbUser).State = System.Data.Entity.EntityState.Modified;
                }
                else
                {
                    //save
                    this.DbContext.Users.Add(dbUser);
                }
                await this.DbContext.SaveChangesAsync();

                userDetails.UserId = Convert.ToUInt32(dbUser.UserId);
                return userDetails;
            }
            catch (Exception)
            {
                //TODO: Handle expection to log into app insights
                throw;
            }
        }

        public async Task<Models.User> GetUserById(int userId)
        {
            var result = new Models.User();
            try
            {
                if (userId > 0)
                {
                    var dbUser = await this.DbContext.Set<DataModel.User>().FindAsync(userId).ConfigureAwait(false);
                    result = Mapper.Map<Models.User>(dbUser);
                }
            }
            catch (Exception)
            {
                //TODO: Handle expection to log into app insights
                throw;
            }
            return result;
        }

        public async Task<List<Models.User>> GetUsers()
        {
            var result = new List<Models.User>();
            try
            {

                var dbUsers = await this.DbContext.Set<DataModel.User>().ToListAsync().ConfigureAwait(false);
                result = Mapper.Map<List<Models.User>>(dbUsers);
            }
            catch (Exception)
            {
                //TODO: Handle expection to log into app insights
                throw;
            }
            return result;
        }

        public Task<Models.User> GetUser(int userId)
        {
            throw new NotImplementedException();
        }
    }
}
