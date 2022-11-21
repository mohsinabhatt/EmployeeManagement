using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
namespace SharedLibrary
{
    public static class EmailHandler
    {
        public static async Task SendMail(MailSetting mailSetting)
        {
            await Execute(mailSetting);
        }


        public static void SendMailInBackground(MailSetting mailSetting)
        {
            Task.Run(() =>
            {
                Execute(mailSetting).ConfigureAwait(false);
            });
        }


        //private static void ProcessMail(MailSetting mailSetting)
        //{
        //    if (mailSetting.To.IsNullOrEmpty() && mailSetting.CC.IsNullOrEmpty())
        //        return;

        //    if (mailSetting.Body.IsNullOrEmpty() && mailSetting.Subject.IsNullOrEmpty() && mailSetting.Attachments.IsNullOrEmpty())
        //        return;

        //    MailMessage mail = new MailMessage
        //    {
        //        From = new MailAddress("logichubss@gmail.com", "Patternsbay"),
        //        Subject = mailSetting.Subject,
        //        Body = mailSetting.Body,
        //        IsBodyHtml = mailSetting.IsBodyHtml
        //    };

        //    mailSetting.To?.ForEach(x => mail.To.Add(new MailAddress(x)));

        //    mailSetting.CC?.ForEach(x => mail.CC.Add(new MailAddress(x)));

        //    mailSetting.Attachments?.ForEach(x => mail.Attachments.Add(x));
           
        //    //SmtpClient client = new SmtpClient("smtp.gmail.com", 587)
        //    //{
        //    //    Credentials = new NetworkCredential("logichubss1@gmail.com", "Logic_Hub@1"),
        //    //    //DeliveryMethod = SmtpDeliveryMethod.Network,
        //    //    EnableSsl = true,
        //    //    //UseDefaultCredentials = false
        //    //};

        //    //client.Send(mail);
        //}


        static async Task Execute(MailSetting mailSetting)
        {
            var apiKey ="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";
            var client = new SendGridClient(apiKey);

            if (mailSetting.To.IsNullOrEmpty() && mailSetting.CC.IsNullOrEmpty())
                return;

            if (mailSetting.Body.IsNullOrEmpty() && mailSetting.Subject.IsNullOrEmpty() && mailSetting.Attachments.IsNullOrEmpty())
                return;

            SendGridMessage mail = new SendGridMessage
            {
                From = new EmailAddress(mailSetting.From?.Address ?? "logichubss1@gmail.com", mailSetting.From?.DisplayName ?? "Patterns"),
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
            //mailSetting.Attachments?.ForEach(x => mail.Attachments.Add(x));

            var response = await client.SendEmailAsync(mail);
        }
    }


    public sealed class MailSetting1
    {
        public List<string> To { get; set; }


        public List<string> CC { get; set; }


        public MailAddress From { get; set; }


        public string Subject { get; set; }


        public string Body { get; set; }


        public bool IsBodyHtml { get; set; }


        public List<System.Net.Mail.Attachment> Attachments { get; set; }
    }
}
