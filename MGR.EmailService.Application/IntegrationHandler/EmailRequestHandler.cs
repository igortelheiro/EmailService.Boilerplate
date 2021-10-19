using System.Threading.Tasks;
using MGR.Common.EventBus.Events;
using MGR.Common.EventBus.Extensions;
using MGR.Common.EventBus.Interfaces;
using MGR.EmailService.Application.Interface;

namespace MGR.EmailService.Application.IntegrationHandler
{
    public class EmailRequestHandler : IIntegrationEventHandler<EmailRequestEvent>
    {
        private readonly IEmailSenderService _emailSenderSenderService;


        public EmailRequestHandler(IEmailSenderService emailSenderSenderService) =>
            _emailSenderSenderService = emailSenderSenderService;


        public async Task Handle(EmailRequestEvent @event)
        {
            var emailRequest = @event.ToEmailRequestDto();
            await _emailSenderSenderService.SendEmailAsync(emailRequest);
        }
    }
}
