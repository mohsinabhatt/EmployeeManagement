using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public sealed class MailSetting
    {
        public List<string> To { get; set; }


        public List<string> CC { get; set; }


        public MailAddress From { get; set; }


        public string Subject { get; set; }


        public string Body { get; set; }


        public bool IsBodyHtml { get; set; }


        public List<System.Net.Mail.Attachment> Attachments { get; set; }
    }


    public abstract class EmailService : IEmailService
    {
        public async Task SendMailAsync(MailSetting mailSetting)
        {
            await SendAsync(mailSetting);
        }


        public void SendMailInBackground(MailSetting mailSetting)
        {
            Task.Run(() =>
            {
                SendAsync(mailSetting).ConfigureAwait(false);
            });
        }


        internal abstract Task SendAsync(MailSetting mailSetting);
    }
}
