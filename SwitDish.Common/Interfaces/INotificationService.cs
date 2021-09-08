using System.Threading.Tasks;

namespace SwitDish.Common.Interfaces
{
    public interface INotificationService
    {
        Task<bool> SendEmailAsync(string emailAddress, string subject, string message);
    }
}
