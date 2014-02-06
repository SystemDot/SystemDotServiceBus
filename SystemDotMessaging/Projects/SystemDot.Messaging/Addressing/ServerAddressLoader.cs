using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;
using SystemDot.Configuration.Reading;
using SystemDot.Http;

namespace SystemDot.Messaging.Addressing
{
    public class ServerAddressLoader
    {
        readonly IConfigurationReader configurationReader;

        public ServerAddressLoader(IConfigurationReader configurationReader)
        {
            Contract.Requires(configurationReader != null);

            this.configurationReader = configurationReader; 
        }

        public ConcurrentDictionary<string, ServerAddress> Load()
        {
            return new ConcurrentDictionary<string, ServerAddress>(
                GetAddressSettingsSection()
                    .ToDictionary(GetNameValue, GetServerAddress));
        }

        IEnumerable<XElement> GetAddressSettingsSection()
        {
            return this.configurationReader.GetSettingsInSection("ServerAddresses");
        }

        static ServerAddress GetServerAddress(XElement node)
        {
            return new ServerAddress(GetAddressValue(node), GetIsSecureValue(node));
        }

        static string GetNameValue(XElement node)
        {
            return node.Attributes("name").Single().Value;
        }

        static string GetAddressValue(XElement node)
        {
            return node.Attributes("address").Single().Value;
        }

        static bool GetIsSecureValue(XElement node)
        {
            return Boolean.Parse(node.Attributes("isSecure").Single().Value);
        }
    }
}