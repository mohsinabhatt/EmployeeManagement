using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace SharedLibrary
{
    public sealed class SendgridConfig
    {
        public string ApiKey { get; set; }


        public string DefaultFromEmail { get; set; }


        public string DefaultFromName { get; set; }
    }


    internal sealed class SendgridEmailService : EmailService
    {
        private readonly SendgridConfig sendgridConfig;


        public SendgridEmailService(IOptions<SendgridConfig> options)
        {
            sendgridConfig = options.Value;
        }


        internal override async Task SendAsync(MailSetting mailSetting)
        {
            var client = new SendGridClient(sendgridConfig.ApiKey);

            if (mailSetting.To.IsNullOrEmpty() && mailSetting.CC.IsNullOrEmpty())
                return;

            if (mailSetting.Body.IsNullOrEmpty() && mailSetting.Subject.IsNullOrEmpty() && mailSetting.Attachments.IsNullOrEmpty())
                return;

            SendGridMessage mail = new SendGridMessage
            {
                From = new EmailAddress(mailSetting.From?.Address ?? sendgridConfig.DefaultFromEmail, mailSetting.From?.DisplayName ?? sendgridConfig.DefaultFromName),
                Subject = mailSetting.Subject,
            };
            if (mailSetting.IsBodyHtml)
            {
                mail.HtmlContent = mailSetting.Body;
            }
            else
            {
                mail.PlainTextContent = mailSetting.Body;
            }

            mailSetting.To?.ForEach(x => mail.AddTo(new EmailAddress(x)));
            mailSetting.CC?.ForEach(x => mail.AddCc(new EmailAddress(x)));
            mailSetting.Attachments?.ForEach(x =>
            {
                using (var reader = new BinaryReader(x.ContentStream))
                {
                    mail.Attachments.Add(new SendGrid.Helpers.Mail.Attachment
                    {
                        Content = Convert.ToBase64String(reader.ReadBytes((int)x.ContentStream.Length)),
                        ContentId = x.ContentId,
                        Disposition = x.ContentDisposition.DispositionType,
                        Filename = x.Name,
                        Type = x.ContentType.MediaType
                    });
                }
            });

            await client.SendEmailAsync(mail);
        }
    }
}
