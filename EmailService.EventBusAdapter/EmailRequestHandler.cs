using System.Threading.Tasks;
using EmailService.Application.Interface;
using EmailService.Application.Model;
using EventBus.Core.Events;
using EventBus.Core.Interfaces;

namespace EmailService.EventBusAdapter;

public class EmailRequestHandler : IIntegrationEventHandler<EmailRequestedEvent>
{
    private readonly IEmailSenderService _emailSenderSenderService;


    public EmailRequestHandler(IEmailSenderService emailSenderSenderService) =>
        _emailSenderSenderService = emailSenderSenderService;


    public async Task Handle(EmailRequestedEvent @event)
    {
        var emailRequest = new EmailRequest
        {
            DestinationEmail = @event.DestinationEmail,
            Subject = @event.Subject,
            Content = @event.Content,
            Template = @event.Template
        };
        await _emailSenderSenderService.SendEmailAsync(emailRequest);
    }
}