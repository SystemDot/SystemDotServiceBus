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

        public EndpointAddress Build(string address, ServerRoute defaultServerRoute)
        {
            Contract.Requires(!string.IsNullOrEmpty(address));
            Contract.Requires(defaultServerRoute != null);

            string channel = GetChannelName(address);
            ServerRoute route = GetServerPath(address, defaultServerRoute);

            return new EndpointAddress(channel, route);
        }

        string GetChannelName(string address)
        {
            return ParseChannel(address)[0];
        }

        ServerRoute GetServerPath(string address, ServerRoute defaultServerRoute)
        {
            if (ParseChannel(address).Length == 1)
                return defaultServerRoute;

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