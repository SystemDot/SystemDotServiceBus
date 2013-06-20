using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Ioc;

namespace SystemDot.Messaging.Specifications
{
    public class TestEndpointAddressBuilder
    {
        public static EndpointAddress Build(string channel, string serverName)
        {
            return new EndpointAddress(
                channel, 
                new ServerPath(MessageServer.Named(serverName, ServerAddress.Local), MessageServer.None));
        }

        public static EndpointAddress Build(string channel, string serverName, string serverAddress)
        {
            return new EndpointAddress(
                channel,
                new ServerPath(
                    MessageServer.Named(serverName, new ServerAddress(serverAddress, false)),
                    MessageServer.Named(serverName, new ServerAddress(serverAddress, false))));
        }

        public static EndpointAddress BuildWithProxy(string channel, string serverName, string proxyName)
        {
            return new EndpointAddress(
                channel,
                new ServerPath(
                    MessageServer.Named(serverName, ServerAddress.Local),
                    MessageServer.Named(proxyName, ServerAddress.Local)));
        }

        
    }
}