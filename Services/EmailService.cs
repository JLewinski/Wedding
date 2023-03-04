using Microsoft.Extensions.Options;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Net;
using System.Net.Mail;
using Wedding.Models;

namespace Wedding.Services
{
    public class EmailOptions
    {
        public string Host { get; init; }
        public int Port { get; init; }
        public string Username { get; init; }
        public string Password { get; init; }
        public bool EnableSsl { get; init; }
        public bool UseCredentials { get; init; }
    }

    public class EmailService : IDisposable
    {
        private readonly ILogger<EmailService> _logger;
        private readonly EmailOptions _options;
        private SmtpClient? _smtpClient;
        private SmtpClient client
        {
            get
            {
                if (_smtpClient == null)
                {
                    _smtpClient ??= new SmtpClient
                    {
                        EnableSsl = _options.EnableSsl,
                        Host = _options.Host,
                        Port = _options.Port,
                        DeliveryMethod = SmtpDeliveryMethod.Network
                    };

                    if (_options.UseCredentials)
                    {
                        _smtpClient.Credentials = new NetworkCredential(_options.Username, _options.Password);
                    }

                }
                _smtpClient ??= new SmtpClient
                {
                    EnableSsl = _options.EnableSsl,
                    Host = _options.Host,
                    Port = _options.Port,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    
                };
                return _smtpClient;
            }
        }

        public EmailService(ILogger<EmailService> logger, IOptions<EmailOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public void Dispose()
        {
            _smtpClient?.Dispose();
        }

        public async Task SendConfirmationEmail(ThankYouViewModel guest, string url)
        {
            var fileText = await System.IO.File.ReadAllTextAsync(@"./wwwroot/dist/email/confirmation.html");
            fileText = fileText.Replace("@Model.Email", guest.Email).Replace("@Model.GuestId", guest.GuestId.ToString()).Replace("@Model.Url", url ?? @"https://jacobandelisa.com");
            var message = new MailMessage
            {
                Subject = "Jacob and Elisa's Wedding",
                From = new MailAddress("wedding@lewinskitech.com", "Jacob and Elisa"),
                IsBodyHtml = true,
                Body = fileText
            };
            message.To.Add(guest.Email);
            await SendMessage(message);
        }

        private async Task SendMessage(MailMessage message)
        {
            try
            {
                await client.SendMailAsync(message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email");
                throw;
            }
        }
    }
}