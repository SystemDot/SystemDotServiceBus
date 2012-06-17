using System.Diagnostics.Contracts;
using SystemDot.Threading;

namespace SystemDot.Messaging.Configuration.Remote
{
    public class RemoteConfiguration
    {
        public UsingDefaultsConfiguration UsingDefaults()
        {
            return new UsingDefaultsConfiguration();    
        }
    }
}