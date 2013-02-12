using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Specifications
{
    public class TestEndpointAddressBuilder
    {
        public static EndpointAddress Build(string channel, string server)
        {
            return new EndpointAddressBuilder().Build(channel, new ServerPath(MessageServer.Named(server), MessageServer.Named(server)));
        }
    }
}