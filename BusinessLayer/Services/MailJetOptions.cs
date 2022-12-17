using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer
{
    public class MailJetOptions
    {
        public string ApiKey { get; set; }

        public string SecretKey { get; set; }
    }

    public class MailJetSettings
    {
        public string FromEmail { get; set; }

        public string FromName { get; set; }

    }
}
