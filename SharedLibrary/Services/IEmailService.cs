using System.Threading.Tasks;
namespace SharedLibrary
{
    public interface IEmailService
    {
        Task SendMailAsync(MailSetting mailSetting);


        void SendMailInBackground(MailSetting mailSetting);
    }
}
