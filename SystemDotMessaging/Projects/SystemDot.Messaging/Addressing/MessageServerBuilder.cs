using System;
using System.Diagnostics.Contracts;
using SystemDot.Environment;
using SystemDot.Http;

namespace SystemDot.Messaging.Addressing
{
    public class MessageServerBuilder
    {
        readonly ServerAddressRegistry serverAddressRegistry;
        readonly ILocalMachine localMachine;

        public MessageServerBuilder(ServerAddressRegistry serverAddressRegistry, ILocalMachine localMachine)
        {
            Contract.Requires(serverAddressRegistry != null);
            this.serverAddressRegistry = serverAddressRegistry;
            this.localMachine = localMachine;
        }

        public MessageServer BuildMultipoint(string server)
        {
            Contract.Requires(!string.IsNullOrEmpty(server));
            
            return MessageServer.NamedMultipoint(server, localMachine.GetName(), GetAddress(server));
        }

        public MessageServer Build(string server)
        {
            Contract.Requires(!string.IsNullOrEmpty(server));

            return ServerHasMachineName(server) ? GetNamedMultipointServer(server) : GetNamedServer(server);
        }

        MessageServer GetNamedServer(string server)
        {
            return MessageServer.Named(server, GetAddress(GetServerName(server)));
        }

        MessageServer GetNamedMultipointServer(string server)
        {
            return MessageServer.NamedMultipoint(GetServerName(server), GetMachineName(server), GetAddress(GetServerName(server)));
        }

        static string GetMachineName(string server)
        {
            return ParseServer(server)[1];
        }

        static string GetServerName(string server)
        {
            return ParseServer(server)[0];
        }

        static bool ServerHasMachineName(string server)
        {
            return ParseServer(server).Length == 2;
        }

        static string[] ParseServer(string server)
        {
            return server.Split('.');
        }

        ServerAddress GetAddress(string server)
        {
            return serverAddressRegistry.Lookup(server);
        }
    }
}