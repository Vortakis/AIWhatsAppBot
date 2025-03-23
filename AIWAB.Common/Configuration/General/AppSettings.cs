using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIWAB.Common.Configuration.General
{
    public class AppSettings
    {
        public Dictionary<string, EndpointSettings> Endpoints { get; set; } = new ()
        {
            { "AIProviderAPI", new () },
            { "QnAAPI", new () }
        };
    }
}
