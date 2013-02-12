using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Addressing
{
    public class EndpointAddressBuilder
    {
        public EndpointAddress Build(string address, ServerPath defaultServerPath)
        {
            Contract.Requires(!string.IsNullOrEmpty(address));
            Contract.Requires(defaultServerPath != null);

            string[] channelParts = address.Split('@');
            if (channelParts.Length == 1)
                return new EndpointAddress(address, defaultServerPath);

            string[] serverPathParts = channelParts[1].Split('.');
            if (serverPathParts.Length == 1)
                return new EndpointAddress(channelParts[0], new ServerPath(MessageServer.Named(serverPathParts[0]), defaultServerPath.RoutedVia));

            return new EndpointAddress(channelParts[0], new ServerPath(MessageServer.Named(serverPathParts[0]), MessageServer.Named(serverPathParts[1])));
        }
    }
}