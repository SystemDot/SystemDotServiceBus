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

            string channel = GetChannelName(address);
            ServerPath path = GetServerPath(address, defaultServerPath);

            return new EndpointAddress(channel, path);
        }

        string GetChannelName(string address)
        {
            return ParseChannel(address)[0];
        }

        ServerPath GetServerPath(string address, ServerPath defaultServerPath)
        {
            if (ParseChannel(address).Length == 1)
                return defaultServerPath;

            string serverPath = ParseChannel(address)[1];
            string[] pathParts = ParseServerPath(serverPath);

            if (pathParts.Length == 1)
                return new ServerPath(MessageServer.Named(serverPath), MessageServer.Named(serverPath));
            
            return new ServerPath(MessageServer.Named(pathParts[0]), MessageServer.Named(pathParts[1]));
        }


        static string[] ParseChannel(string address)
        {
            return address.Split('@');
        }

        static string[] ParseServerPath(string serverPath)
        {
            return serverPath.Split('.');
        }
    }
}