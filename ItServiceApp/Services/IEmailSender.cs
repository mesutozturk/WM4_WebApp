using System.Threading.Tasks;
using ItServiceApp.Models;

namespace ItServiceApp.Services
{
    public interface IEmailSender
    {
        Task SendAsync(EmailMessage message);
    }
}
