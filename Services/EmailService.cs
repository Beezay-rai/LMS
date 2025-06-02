using LMS.Interfaces;
using LMS.Models.Settings;
using SendGrid;
using SendGrid.Helpers.Mail;
using SendGrid.Helpers.Mail.Model;

namespace LMS.Services
{
    public class EmailService : IEmailService
    {
        private readonly SendGridSetting _sendGrid;
        private readonly SendGridClient client;
        public EmailService(SendGridSetting sendGridSetting)
        {
            _sendGrid = sendGridSetting;
            client = new SendGridClient(_sendGrid.ApiKey);
        }
        public  void SendMessage()
        {
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("eminemrae@gmail.com", "DX Team"),
                Subject = "Sending with Twilio SendGrid is Fun",
                PlainTextContent = "and easy to do anywhere, even with C#",
                HtmlContent = "<strong>and easy to do anywhere, even with C#</strong>"
            };
            msg.AddTo(new EmailAddress("beezayrae@gmail.com", "Test User"));
            var response =  client.SendEmailAsync(msg);

        }
    }
}
