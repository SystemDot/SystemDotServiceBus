using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Specifications
{
    public class TestEndpointAddressBuilder
    {
        public static EndpointAddress Build(string channel, string serverName)
        {
            return new EndpointAddress(channel, MessageServer.Named(serverName, ServerAddress.Local));
        }

        public static EndpointAddress Build(string channel, string serverName, string serverAddress)
        {
            return new EndpointAddress(channel, MessageServer.Named(serverName, new ServerAddress(serverAddress, false)));
        }
    }
}