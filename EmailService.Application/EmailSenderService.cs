using EmailService.Application.Interface;
using EmailService.Application.Model;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace EmailService.Application
{
    public class EmailSenderService : IEmailSenderService
    {
        #region Intialize
        private readonly IFluentEmail _mailer;
        private readonly SmtpConfiguration _smtpConfig;
        private readonly ILogger<EmailSenderService> _logger;

        public EmailSenderService(IFluentEmail mailer,
                                  IOptions<SmtpConfiguration> smtpConfig,
                                  ILogger<EmailSenderService> logger)
        {
            _mailer = mailer;
            _logger = logger;
            _smtpConfig = smtpConfig.Value;
        }
        #endregion

        
        public async Task<EmailSendResult> SendEmailAsync(EmailRequest request)
        {
            var result = new EmailSendResult();
            try
            {
                BuildEmail(request);

                var response = await _mailer.SendAsync();
                if (!response.Successful)
                {
                    return result with { Error = response.ErrorMessages.FirstOrDefault() };
                }

                return result;
            }
            catch (Exception ex)
            {
                return result with { Error = ex.Message };
            }
        }


        private void BuildEmail(EmailRequest request)
        {
            var emailData = new EmailData
            {
                Subject = request.Subject,
                ToAddresses = { new Address(request.DestinationEmail) },
                Body = request.Content
            };

            _mailer.Data = emailData;
            _mailer.SetFrom(_smtpConfig.SenderEmail, _smtpConfig.SenderName);

            if (request.Template != null)
            {
                _mailer.UsingTemplate(request.Template, new object());
            }
        }
    }
}
