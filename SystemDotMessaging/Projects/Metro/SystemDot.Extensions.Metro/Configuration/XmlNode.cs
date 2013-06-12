using System.Collections.Generic;
using Windows.Data.Xml.Dom;
using System.Linq;

namespace SystemDot.Configuration
{
    public class XmlNode
    {
        public Dictionary<string, XmlAttribute> Attributes { get; private set; }

        public XmlNode(IXmlNode node)
        {
            Attributes = node.Attributes.ToDictionary(a => a.NodeName.ToString(), a => new XmlAttribute(a));
        }
    }
}