using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Specifications
{
    public class TestEndpointAddressBuilder
    {
        public static EndpointAddress Build(string channel, string server)
        {
            return new EndpointAddressBuilder().Build(
                channel, 
                new ServerPath(
                    MessageServer.Named(server),
                    MessageServer.None));
        }

        public static EndpointAddress Build(string channel, string server, string proxyInstance)
        {
            return new EndpointAddressBuilder().Build(
                channel,
                new ServerPath(
                    MessageServer.Named(server),
                    MessageServer.Named(proxyInstance)));
        }
    }
}