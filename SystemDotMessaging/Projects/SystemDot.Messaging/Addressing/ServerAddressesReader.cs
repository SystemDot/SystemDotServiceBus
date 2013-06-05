using System.Collections.Generic;
using SystemDot.Configuration;

namespace SystemDot.Messaging.Addressing
{
    public class ServerAddressesReader : IServerAddressesReader
    {
        readonly ConfigurationReader configurationReader;

        public ServerAddressesReader(ConfigurationReader configurationReader)
        {
            this.configurationReader = configurationReader;
        }

        public Dictionary<string, string> LoadAddresses()
        {
            this.configurationReader.Load("SystemDot.config");
            return this.configurationReader.GetSettingsInSection("ServerAddresses");
        }
    }
}