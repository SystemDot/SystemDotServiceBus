using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Addressing
{
    public class EndpointAddressBuilder
    {
        readonly MessageServerBuilder messageServerBuilder;

        public EndpointAddressBuilder(MessageServerBuilder messageServerBuilder)
        {
            Contract.Requires(messageServerBuilder != null);

            this.messageServerBuilder = messageServerBuilder;
        }

        public EndpointAddress Build(string address, MessageServer defaultServer)
        {
            Contract.Requires(!string.IsNullOrEmpty(address));
            Contract.Requires(defaultServer != null);

            string channel = GetChannelName(address);
            MessageServer route = GetServer(address, defaultServer);

            return new EndpointAddress(channel, route);
        }

        string GetChannelName(string address)
        {
            return ParseChannel(address)[0];
        }

        MessageServer GetServer(string address, MessageServer defaultServer)
        {
            if (ParseChannel(address).Length == 1)
                return defaultServer;

            return messageServerBuilder.Build(ParseChannel(address)[1]);
        }

        static string[] ParseChannel(string address)
        {
            return address.Split('@');
        }
    }
}