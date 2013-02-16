using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Specifications
{
    public class TestEndpointAddressBuilder
    {
        public static EndpointAddress Build(string channel, string serverInstance)
        {
            return new EndpointAddressBuilder().Build(
                channel, 
                new ServerPath(
                    MessageServer.Local(serverInstance),
                    MessageServer.None));
        }

        public static EndpointAddress Build(string channel, string serverInstance, string proxyInstance)
        {
            return new EndpointAddressBuilder().Build(
                channel,
                new ServerPath(
                    MessageServer.Local(serverInstance),
                    MessageServer.Local(proxyInstance)));
        }
    }
}