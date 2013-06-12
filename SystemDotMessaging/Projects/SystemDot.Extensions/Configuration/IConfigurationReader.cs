using System.Collections.Generic;
using System.Xml;

namespace SystemDot.Configuration
{
    public interface IConfigurationReader
    {
        void Load(string fileName);
        IEnumerable<XmlNode> GetSettingsInSection(string section);
    }
}