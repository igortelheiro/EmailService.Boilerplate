using EmailService.Application.Model;
using System.Threading.Tasks;

namespace EmailService.Application.Interface
{
    public interface IEmailSenderService
    {
        Task<EmailSendResult> SendEmailAsync(EmailRequest request);
    }
}
