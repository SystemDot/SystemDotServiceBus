using System.Diagnostics.Contracts;
using SystemDot.Messaging.Configuration.ComponentRegistration;

namespace SystemDot.Messaging.Configuration
{
    public class MessageServerConfiguration : Configurer
    {
        public ChannelConfiguration OpenChannel(string name)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));

            Components.Register();
            
            return new ChannelConfiguration(BuildEndpointAddress(name));
        }
    }
}