using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using SystemDot.Messaging.Configuration.Remote;
using SystemDot.Threading;

namespace SystemDot.Messaging.Configuration.Local
{
    public class LocalConfiguration
    {
        public UsingDefaultsConfiguration UsingDefaults()
        {
            return new UsingDefaultsConfiguration();
        }
    }
}