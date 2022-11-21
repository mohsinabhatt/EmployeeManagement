using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;

namespace SharedLibrary
{
    public interface IAppLogger
    {
        void LogToEmail(Exception ex);
    }


    public sealed class Logger : IAppLogger
    {
        private readonly LoggerOptions loggerOptions;
        private readonly IEmailService emailService;

        public Logger(IEmailService emailService, IOptions<LoggerOptions> options)
        {
            loggerOptions = options.Value;
            this.emailService = emailService;
        }


        public void LogToEmail(Exception ex)
        {
            Task.Run(() =>
            {
                if (loggerOptions.LogTo == LogTo.Email)
                {
                    var mailSetting = new MailSetting
                    {
                        From = new MailAddress(loggerOptions.FromEmail, loggerOptions.FromName),
                        To = loggerOptions.To,
                        CC = loggerOptions.CC,
                        Body = JsonConvert.SerializeObject(ex, Formatting.Indented),
                        Subject = ex.Message
                    };

                    //EmailHandler.SendMail(mailSetting).ConfigureAwait(false);
                    emailService.SendMailAsync(mailSetting).ConfigureAwait(false);
                }
            });
        }
    }


    public sealed class LoggerOptions
    {
        public LogTo LogTo { get; set; }


        public List<string> To { get; set; }


        public List<string> CC { get; set; }


        public string FromEmail { get; set; }


        public string FromName { get; set; }
    }


    public static partial class DI
    {
        public static IServiceCollection AddAppLogger(this IServiceCollection services, LoggerOptions options)
        {
            services.Configure<LoggerOptions>(x =>
            {
                x.FromEmail = options.FromEmail;
                x.CC = options.CC;
                x.FromName = options.FromName;
                x.LogTo = options.LogTo;
                x.To = options.To;
            });
            services.AddSingleton<IAppLogger, Logger>();
            return services;
        }
    }
}
