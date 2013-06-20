using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Addressing
{
    public class EndpointAddressBuilder
    {
        readonly ServerPathBuilder serverPathBuilder;

        public EndpointAddressBuilder(ServerPathBuilder serverPathBuilder)
        {
            Contract.Requires(serverPathBuilder != null);

            this.serverPathBuilder = serverPathBuilder;
        }

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

            return pathParts.Length == 1 
                ? this.serverPathBuilder.Build(serverPath, serverPath) 
                : this.serverPathBuilder.Build(pathParts[0], pathParts[1]);
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