using BusinessLayer.Services;
using Castle.Core.Smtp;
using Mailjet.Client;
using Microsoft.Extensions.Configuration;
using MimeKit.Cryptography;
using Newtonsoft.Json.Linq;
using SharedLibrary;
using System.Net.Mail;

namespace BusinessLayer
{
    public class MailJetSender : IEmailSender
    {
        private readonly IConfiguration configuration;
        public MailJetOptions options;
        public MailJetSettings settings;

        public MailJetSender(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public async void Send(string from, string to, string subject, string messageText)
        {
            var api = configuration.GetSection("MailJetOptions")["ApiKey"];
            var secretKey = configuration.GetSection("MailJetOptions")["SecretKey"];

            var email = configuration.GetSection("MailJetSetting")["FromMail"];
            var name = configuration.GetSection("MailJetSetting")["FromName"];
            MailjetClient mailjetClient = new MailjetClient(api, secretKey);
            MailjetRequest mailjetRequest = new MailjetRequest()
            {
                Resource = Sender.Resource,
            }
            .Property(Sender.FromEmail, email)
            .Property(Sender.FromName, name)
            .Property(Sender.Subject, subject)
            .Property(Sender.HtmlPart, messageText)
            .Property(Sender.Recipients, new JArray
            {
                new JObject
                {
                    {
                        "Email",to
                    }
                }
            });
            await mailjetClient.PostAsync(mailjetRequest);

        }

        public void Send(MailMessage message)
        {
            throw new NotImplementedException();
        }

        public void Send(IEnumerable<MailMessage> messages)
        {
            throw new NotImplementedException();
        }
    }
}
