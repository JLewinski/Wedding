using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
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

        public async Task SendConfirmationEmail(ThankYouViewModel guest, string body, string url)
        {
            var message = new MailMessage
            {
                Subject = "Jacob and Elisa's Wedding",
                From = new MailAddress("wedding@lewinskitech.com", "Jacob and Elisa"),
                IsBodyHtml = true,
                Body = body
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

        public static async Task<string> RenderViewToStringAsync(string viewName, object model, ControllerContext controllerContext, bool isPartial = false)
        {
            var actionContext = controllerContext as ActionContext;

            var serviceProvider = controllerContext.HttpContext.RequestServices;
            var razorViewEngine = serviceProvider.GetService(typeof(IRazorViewEngine)) as IRazorViewEngine;
            var tempDataProvider = serviceProvider.GetService(typeof(ITempDataProvider)) as ITempDataProvider;

            using (var sw = new StringWriter())
            {
                var viewResult = razorViewEngine.FindView(actionContext, viewName, !isPartial);

                if (viewResult?.View == null)
                    throw new ArgumentException($"{viewName} does not match any available view");

                var viewDictionary =
                    new ViewDataDictionary(new EmptyModelMetadataProvider(),
                        new ModelStateDictionary())
                    { Model = model };

                var viewContext = new ViewContext(
                    actionContext,
                    viewResult.View,
                    viewDictionary,
                    new TempDataDictionary(actionContext.HttpContext, tempDataProvider),
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();
            }
        }
    }
}