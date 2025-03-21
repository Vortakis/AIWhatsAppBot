using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWAB.Common.Configuration.ExternalMsgPlatform;

public class ExternalMsgPlatformSettings
{
    public string DefaultPlatform { get; set; } = "";

    public Dictionary<string, MessagingPlatformSettings> MessagingPlatforms { get; set; } = [];
}
