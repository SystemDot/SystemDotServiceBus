using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml;

namespace SystemDot.Configuration
{
    public class ConfigurationReader : IConfigurationReader
    {
        XmlDocument document;
        
        public void Load(string fileName)
        {
            this.document = new XmlDocument();

            using (var reader = XmlReader.Create(MainActivityLocator.Locate().Assets.Open(fileName)))
                this.document.Load(reader);
        }

        public IEnumerable<XmlNode> GetSettingsInSection(string section)
        {
            Contract.Assert(!string.IsNullOrEmpty(section));

            return this.document == null
                ? new List<XmlNode>()
                : this.document.SelectSingleNode(section).ChildNodes.Cast<XmlNode>();
        }
    }
}