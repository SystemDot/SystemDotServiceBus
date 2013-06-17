using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using SystemDot.Http;

namespace SystemDot.Messaging.Addressing
{
    public class ServerAddressRegistry
    {
        readonly ConcurrentDictionary<string, ServerAddress> addresses;

        public ServerAddressRegistry(ServerAddressLoader loader)
        {
            Contract.Requires(loader != null);

            this.addresses = loader.Load();
        }

        public void Register(string name, ServerAddress address)
        {
            Contract.Requires(!String.IsNullOrEmpty(name));
            Contract.Requires(address != null);

            this.addresses.TryAdd(name, address);
        }

        public ServerAddress Lookup(string toLookup)
        {
            Contract.Requires(!String.IsNullOrEmpty(toLookup));

            return this.addresses.ContainsKey(toLookup) && this.addresses[toLookup].Address != string.Empty
                ? this.addresses[toLookup]
                : ServerAddress.Local;
        }

        public FixedPortAddress Lookup(ServerPath toLookup)
        {
            Contract.Requires(toLookup != null);

            return new FixedPortAddress(Lookup(toLookup.Proxy.Name), toLookup.Proxy.Name);
        }
    }
}