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
            return address.Split('@')[0];
        }

        ServerPath GetServerPath(string address, ServerPath defaultServerPath)
        {
            if (address.Split('@').Length == 1)
                return defaultServerPath;

            string serverPath = address.Split('@')[1];

            if(serverPath.Split('.').Length == 1)
                return new ServerPath(GetMessageServer(serverPath), defaultServerPath.Proxy);

            return new ServerPath(GetMessageServer(serverPath.Split('.')[0]), GetMessageServer(serverPath.Split('.')[1]));
        }

        MessageServer GetMessageServer(string server)
        {
            return MessageServer.Named(server.Split('/')[0], server.Split('/')[1]);
        }
    }   
}