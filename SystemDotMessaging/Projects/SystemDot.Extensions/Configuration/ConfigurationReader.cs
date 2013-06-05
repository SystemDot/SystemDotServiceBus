using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Xml;

namespace SystemDot.Configuration
{
    public class ConfigurationReader
    {
        XmlDocument document;

        public void Load(string fileName)
        {
            if (!File.Exists(fileName)) return;
            
            this.document = new XmlDocument();

            using (var reader = XmlReader.Create(fileName))
                this.document.Load(reader);
        }

        public Dictionary<string, string> GetSettingsInSection(string section)
        {
            Contract.Assert(!string.IsNullOrEmpty(section));

            return GetSettingNodesInSection(section)
                .ToDictionary(node => node.Name, node => node.InnerText);
        }

        IEnumerable<XmlNode> GetSettingNodesInSection(string section)
        {
            return this.document == null 
                ? new List<XmlNode>() 
                : this.document.SelectSingleNode(section).ChildNodes.Cast<XmlNode>();
        }
    }
}