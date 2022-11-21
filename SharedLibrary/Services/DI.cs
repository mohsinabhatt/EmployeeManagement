using Microsoft.Extensions.DependencyInjection;

namespace SharedLibrary.Services
{
    public static class DI
    {
        public static void AddEmailService(this IServiceCollection services, AppEmailConfig config)
        {
            services.Configure<AppEmailConfig>(x =>
            {
                x.DisplayName = config.DisplayName;
                x.EnableSsl = config.EnableSsl;
                x.From = config.From;
                x.Host = config.Host;
                x.Password = config.Password;
                x.Port = config.Port;
                x.Timeout = config.Timeout;
            });
            services.AddSingleton<IEmailService, AppEmailService>();
        }


        public static void AddEmailService(this IServiceCollection services, SendgridConfig config)
        {
            services.Configure<SendgridConfig>(x =>
            {
                x.ApiKey = config.ApiKey;
                x.DefaultFromEmail = config.DefaultFromEmail;
                x.DefaultFromName = config.DefaultFromName;
            });
            services.AddSingleton<IEmailService, SendgridEmailService>();
        }
    }
}
