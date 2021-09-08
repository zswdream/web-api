using SwitDish.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwitDish.Domain.Contract
{
    public interface IUserService
    {
        Task<User> GetUserById(int userId);
        Task<User> GetUser(int userId);
        Task<List<User>> GetUsers();
        Task<Models.User> SaveOrUpdate(User userDetails);

    }
}
