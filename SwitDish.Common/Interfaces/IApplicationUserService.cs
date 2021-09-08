using SwitDish.Common.ViewModels;
using SwitDish.Common.DomainModels;
using System.Threading.Tasks;

namespace SwitDish.Common.Interfaces
{
    public interface IApplicationUserService
    {
        Task<ApplicationUser> GetUser(int applicationUserId);
        Task<ApplicationUser> FindUserByCredentials(string username, string password);
        Task<string> SendPasswordResetEmail(string emailAddress);
        Task<string> ResetPassword(PasswordResetViewModel data);
        Task<string> UpdatePassword(PasswordUpdateViewModel data);
    }
}
