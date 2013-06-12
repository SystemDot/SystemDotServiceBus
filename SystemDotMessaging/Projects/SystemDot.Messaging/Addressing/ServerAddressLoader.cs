using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml;
using SystemDot.Configuration;

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
            this.configurationReader.Load("SystemDot.config");

            return new ConcurrentDictionary<string, ServerAddress>(
                GetAddressSettingsSection()
                    .ToDictionary(GetNameValue, GetServerAddress));
        }

        IEnumerable<XmlNode> GetAddressSettingsSection()
        {
            return this.configurationReader.GetSettingsInSection("ServerAddresses");
        }

        static ServerAddress GetServerAddress(XmlNode node)
        {
            return new ServerAddress(GetAddressValue(node), GetIsSecureValue(node));
        }

        static string GetNameValue(XmlNode node)
        {
            return node.Attributes["name"].Value;
        }

        static string GetAddressValue(XmlNode node)
        {
            return node.Attributes["address"].Value;
        }

        static bool GetIsSecureValue(XmlNode node)
        {
            return Boolean.Parse(node.Attributes["isSecure"].Value);
        }
    }
}