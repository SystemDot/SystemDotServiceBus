using System.Collections.Generic;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Specifications
{
    public class TestServerAddressesReader : IServerAddressesReader
    {
        readonly Dictionary<string, string> addresses;

        public TestServerAddressesReader()
        {
            this.addresses = new Dictionary<string, string>();
        }

        public Dictionary<string, string> LoadAddresses()
        {
            return this.addresses;
        }

        public void AddAddress(string serverName, string serverAddress)
        {
            this.addresses.Add(serverName, serverAddress);
        }
    }
}