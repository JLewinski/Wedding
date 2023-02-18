using System.Net;
using System.Net.Mail;
using Wedding.Models;

namespace Wedding.Services
{
    public class EmailService
    {
        public static async Task SendConfirmationEmail(ThankYouViewModel guest, string url)
        {
            var fileText = await System.IO.File.ReadAllTextAsync(@"./wwwroot/dist/email/confirmation.html");
            fileText = fileText.Replace("@Model.Email", guest.Email).Replace("@Model.GuestId", guest.GuestId.ToString()).Replace("@Model.Url", url);
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

        private static async Task SendMessage(MailMessage message)
        {
            using (var client = new SmtpClient())
            {
                client.EnableSsl = true;
                client.Host = "smtp.office365.com";
                client.Port = 587;
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Credentials = new NetworkCredential("jacob@lewinskitech.com", "z3NI94ZnWvxO");
                await client.SendMailAsync(message);
            }
        }
    }
}