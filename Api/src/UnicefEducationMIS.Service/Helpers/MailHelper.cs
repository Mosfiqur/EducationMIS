using MimeKit;
using UnicefEducationMIS.Core.Settings;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace UnicefEducationMIS.Service.Helpers
{
    public class MailHelper
    {
        private readonly MailSettings _settings;

        public MailHelper(MailSettings settings)
        {
            _settings = settings;
        }

        private MimeMessage CreateMimeMessageFromEmailMessage(EmailMessage message)
        {
            var mimeMessage = new MimeMessage(); 
            mimeMessage.From.Add(message.Sender);
            mimeMessage.To.Add(message.Reciever);
            mimeMessage.Subject = message.Subject;
            var bodyBuilder = new BodyBuilder {HtmlBody = message.Content};
            mimeMessage.Body = bodyBuilder.ToMessageBody(); 
            return mimeMessage;
        }

        public void SendEmail(string sendTo, string subject, string body)
        {
            var mailMsg = new EmailMessage();
            mailMsg.Sender = new MailboxAddress(_settings.Title, _settings.Sender);
            mailMsg.Reciever = new MailboxAddress(_settings.Title, sendTo);
            mailMsg.Subject = subject;
            mailMsg.Content = body;
            var mimeMessage = CreateMimeMessageFromEmailMessage(mailMsg);

            using (SmtpClient client = new SmtpClient())
            {
                client.Connect(_settings.SmtpServer, _settings.Port, false); 
                client.Authenticate(_settings.UserName, _settings.Password); 
                client.Send(mimeMessage); 
                client.Disconnect(true);
            }
        }
    }
}
