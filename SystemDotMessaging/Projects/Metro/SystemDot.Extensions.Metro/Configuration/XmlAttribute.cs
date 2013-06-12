using Windows.Data.Xml.Dom;

namespace SystemDot.Configuration
{
    public class XmlAttribute
    {
        public string Value { get; set; }

        public XmlAttribute(IXmlNode node)
        {
            this.Value = node.NodeValue.ToString();
        }
    }
}