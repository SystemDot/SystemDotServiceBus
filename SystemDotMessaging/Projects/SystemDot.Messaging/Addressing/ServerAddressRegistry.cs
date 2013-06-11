using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using SystemDot.Http;

namespace SystemDot.Messaging.Addressing
{
    public class ServerAddressRegistry
    {
        readonly ConcurrentDictionary<string, string> addresses;

        public ServerAddressRegistry(IServerAddressesReader serverAddressesReader)
        {
            Contract.Requires(serverAddressesReader != null);

            this.addresses = new ConcurrentDictionary<string, string>(serverAddressesReader.LoadAddresses());
        }

        public void Register(string name, string address)
        {
            this.addresses.TryAdd(name, address);
        }

        public string Lookup(string toLookup)
        {
            Contract.Requires(!String.IsNullOrEmpty(toLookup));

            return this.addresses.ContainsKey(toLookup)
                ? this.addresses[toLookup]
                : Environment.MachineName;
        }

        public FixedPortAddress Lookup(ServerPath toLookup)
        {
            Contract.Requires(toLookup != null);

            return new FixedPortAddress(Lookup(toLookup.Proxy.Name), toLookup.Proxy.Name);
        }
    }
}