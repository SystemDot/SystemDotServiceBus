using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Http;

namespace SystemDot.Messaging.Addressing
{
    public class ServerAddressRegistry
    {
        readonly Dictionary<string, string> addresses;

        public ServerAddressRegistry(IServerAddressesReader serverAddressesReader)
        {
            Contract.Requires(serverAddressesReader != null);
            this.addresses = serverAddressesReader.LoadAddresses();
        }

        public void Register(string name, string address)
        {
            this.addresses.Add(name, address);
        }

        public FixedPortAddress Lookup(ServerPath toLookup)
        {
            Contract.Requires(toLookup != null);

            return new FixedPortAddress(GetAddress(toLookup), toLookup.Proxy.Name);
        }

        string GetAddress(ServerPath toLookup)
        {
            return this.addresses.ContainsKey(toLookup.Proxy.Name) 
                ? this.addresses[toLookup.Proxy.Name] 
                : Environment.MachineName;
        }
    }
}