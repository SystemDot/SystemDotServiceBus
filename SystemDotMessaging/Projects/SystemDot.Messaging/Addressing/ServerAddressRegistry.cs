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

            return Contains(toLookup) ? addresses[toLookup] : ServerAddress.Local;
        }

        public bool Contains(string toCheck)
        {
            Contract.Requires(!String.IsNullOrEmpty(toCheck));
            
            return addresses.ContainsKey(toCheck) && addresses[toCheck].Path != string.Empty;
        }
    }
}