using System.Threading.Tasks;
using MGR.Common.Api;
using MGR.Common.Api.DTOs.Base;
using MGR.Common.Api.DTOs.Extensions;
using MGR.Common.Api.DTOs.LoginAPI;
using MGR.EmailService.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace MGR.EmailService.API.Controllers
{
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly ILogger<EmailController> _logger;
        private readonly IEmailSenderService _emailSenderService;

        public EmailController(ILogger<EmailController> logger, IEmailSenderService emailSenderService)
        {
            _logger = logger;
            _emailSenderService = emailSenderService;
        }
        

        [HttpPost(EmailEndpoints.Send)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ResponseDto>> SendEmail(EmailRequestDto emailRequest)
        {
            var result = await _emailSenderService.SendEmailAsync(emailRequest);
            if (result.Sucesso)
            {
                return Ok(result);
            }
            return BadRequest(result.ToProblemDetails("Erro ao tentar enviar email"));
        }
    }
}