using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWAB.Common.Configuration.ExternalMsgPlatform;

public class MessagingPlatformSettings
{
    public string? AuthToken { get; set; }

    public string? PhoneNumber { get; set; }

    public string? AccountSid { get; set; }
}
