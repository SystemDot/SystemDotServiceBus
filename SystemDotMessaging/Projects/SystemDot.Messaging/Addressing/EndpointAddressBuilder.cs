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

            return new EndpointAddress(GetChannelName(address), GetServer(address, defaultServer));
        }

        string GetChannelName(string address)
        {
            return ParseChannel(address)[0];
        }

        MessageServer GetServer(string address, MessageServer defaultServer)
        {
            return AddressHasServerName(address)
                ? messageServerBuilder.Build(GetServerName(address)) 
                : defaultServer;
        }

        static bool AddressHasServerName(string address)
        {
            return ParseChannel(address).Length == 2;
        }

        static string GetServerName(string address)
        {
            return ParseChannel(address)[1];
        }

        static string[] ParseChannel(string address)
        {
            return address.Split('@');
        }
    }
}