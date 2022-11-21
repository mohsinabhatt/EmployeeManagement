using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace SharedLibrary
{
    public sealed class AppEmailConfig
    {
        public string From { get; set; }


        public string DisplayName { get; set; }


        public string Password { get; set; }


        public string Host { get; set; }


        public int Port { get; set; }


        public bool EnableSsl { get; set; }


        public int Timeout { get; set; }
    }


    internal sealed class AppEmailService : EmailService
    {
        private readonly AppEmailConfig config;

        public AppEmailService(IOptions<AppEmailConfig> options)
        {
            config = options.Value;
        }


        internal override async Task SendAsync(MailSetting mailSetting)
        {
            
            if (mailSetting.To.IsNullOrEmpty() && mailSetting.CC.IsNullOrEmpty())
                return;

            if (mailSetting.Body.IsNullOrEmpty() && mailSetting.Subject.IsNullOrEmpty() && mailSetting.Attachments.IsNullOrEmpty())
                return;

            MailMessage mail = new MailMessage
            {
                From = mailSetting.From ?? new MailAddress(config.From, config.DisplayName),
                Subject = mailSetting.Subject,
                Body = mailSetting.Body,
                IsBodyHtml = mailSetting.IsBodyHtml
            };
          
            mailSetting.To?.ForEach(x => mail.To.Add(new MailAddress(x)));
            mailSetting.CC?.ForEach(x => mail.CC.Add(new MailAddress(x)));
            mailSetting.Attachments?.ForEach(x => mail.Attachments.Add(x));

            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = config.Host;
            smtpClient.Port = config.Port;
            smtpClient.EnableSsl = config.EnableSsl;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;

            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = config.From,
                Password = config.Password
            };
            smtpClient.Timeout = config.Timeout;
            await smtpClient.SendMailAsync(mail);
        }
    }
}
