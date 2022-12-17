using Mailjet.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public static class Sender
    {
        public static readonly ResourceInfo Resource = new("send", null, ApiVersion.V3, ResourceType.Send);

        public const string FromEmail = "FromEmail";

        public const string FromName = "FromName";

        public const string Subject = "Subject";

        public const string Recipients = "Recipients";

        public const string HtmlPart = "Html-Part";
    }
}
