using System;
using System.Linq;
using System.Threading.Tasks;
using FluentEmail.Core;
using FluentEmail.Core.Models;
using MGR.Common.Api.DTOs.Base;
using MGR.Common.Api.DTOs.LoginAPI;
using MGR.EmailService.Application.Interface;
using MGR.EmailService.Common;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace MGR.EmailService.Application
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

        
        public async Task<ResponseDto> SendEmailAsync(EmailRequestDto request)
        {
            var result = new ResponseDto();
            try
            {
                BuildEmail(request);

                var response = await _mailer.SendAsync();
                if (!response.Successful)
                {
                    return result with { Erro = response.ErrorMessages.FirstOrDefault() };
                }

                return result;
            }
            catch (Exception ex)
            {
                return result with { Erro = ex.Message };
            }
        }


        private void BuildEmail(EmailRequestDto request)
        {
            var emailData = new EmailData
            {
                Subject = request.Subject,
                ToAddresses = { new Address(request.DestinationEmail) },
                Body = request.Body
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
