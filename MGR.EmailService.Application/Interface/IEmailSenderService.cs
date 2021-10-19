using System.Threading.Tasks;
using MGR.Common.Api.DTOs.Base;
using MGR.Common.Api.DTOs.LoginAPI;

namespace MGR.EmailService.Application.Interface
{
    public interface IEmailSenderService
    {
        Task<ResponseDto> SendEmailAsync(EmailRequestDto request);
    }
}
