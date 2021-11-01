using EmailService.Application.Interface;
using EmailService.Application.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EmailService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MailController : ControllerBase
    {
        private readonly ILogger<MailController> _logger;
        private readonly IEmailSenderService _emailSenderService;

        public MailController(ILogger<MailController> logger, IEmailSenderService emailSenderService)
        {
            _logger = logger;
            _emailSenderService = emailSenderService;
        }
        

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<EmailSendResult>> SendEmail(EmailRequest emailRequest)
        {
            var result = await _emailSenderService.SendEmailAsync(emailRequest);
            if (result.Success)
            {
                return Ok(result);
            }

            return BadRequest(new ProblemDetails
            {
                Title = "Erro ao tentar enviar email",
                Detail = result.Error
            });
        }
    }
}