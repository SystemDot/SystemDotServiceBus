using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using SystemDot.Configuration;
using SystemDot.Configuration.Reading;

namespace SystemDot.Messaging.Specifications
{
    public class TestServerAddressConfigurationReader : IConfigurationReader
    {
        readonly XmlDocument document;
        bool isLoaded;

        public TestServerAddressConfigurationReader()
        {
            this.document = new XmlDocument();
            this.document.AppendChild(this.document.CreateElement("xml"));
        }

        public void AddAddress(string serverName, string serverAddress)
        {
            this.document.FirstChild.AppendChild(CreateServerAddressNode(serverName, serverAddress, false));
        }

        public void AddSecureAddress(string serverName, string serverAddress)
        {
            this.document.FirstChild.AppendChild(CreateServerAddressNode(serverName, serverAddress, true));
        }

        XmlElement CreateServerAddressNode(string serverName, string serverAddress, bool isSecure)
        {
            XmlElement node = this.document.CreateElement("ServerAddress");
            node.Attributes.Append((CreateAttribute("name", serverName)));
            node.Attributes.Append(CreateAttribute("address", serverAddress));
            node.Attributes.Append(CreateAttribute("isSecure", isSecure.ToString()));              
         
            return node;
        }

        XmlAttribute CreateAttribute(string name, string value)
        {
            XmlAttribute attribute = this.document.CreateAttribute(name);
            attribute.Value = value;
 
            return attribute;
        }

        public void Load(string fileName)
        {
            this.isLoaded = (fileName == "SystemDot.config");
        }

        public IEnumerable<XElement> GetSettingsInSection(string section)
        {
            throw new Exception();
            //return section == "ServerAddresses" && this.isLoaded
            //    ? this.document.FirstChild.ChildNodes.OfType<XmlNode>()
            //    : new List<XmlNode>();
        }
    }

}