using System.Collections.Generic;
using System.Xml.Linq;
using SystemDot.Configuration.Reading;

namespace SystemDot.Messaging.Specifications
{
    public class TestServerAddressConfigurationReader : IConfigurationReader
    {
        readonly XElement rootNode;
        bool isLoaded;

        public TestServerAddressConfigurationReader()
        {
            rootNode = new XElement("xml");
        }
        
        public void AddAddress(string serverName, string serverAddress)
        {
            rootNode.Add(CreateServerAddressNode(serverName, serverAddress, false));
        }

        public void AddSecureAddress(string serverName, string serverAddress)
        {
            rootNode.Add(CreateServerAddressNode(serverName, serverAddress, true));
        }

        XElement CreateServerAddressNode(string serverName, string serverAddress, bool isSecure)
        {
            var node = new XElement("ServerAddress");
            node.Add(CreateAttribute("name", serverName));
            node.Add(CreateAttribute("address", serverAddress));
            node.Add(CreateAttribute("isSecure", isSecure.ToString()));              
         
            return node;
        }

        XAttribute CreateAttribute(string name, string value)
        {
            return new XAttribute(name, value);
        }

        public void Load(string fileName)
        {
            isLoaded = (fileName == "SystemDot.config");
        }

        public IEnumerable<XElement> GetSettingsInSection(string section)
        {
            return section == "ServerAddresses" && isLoaded
                ? rootNode.Descendants()
                : new List<XElement>();
        }
    }

}