using System.Collections.Generic;

namespace SystemDot.Configuration
{
    public interface IConfigurationReader
    {
        void Load(string fileName);
        IEnumerable<XmlNode> GetSettingsInSection(string section);
    }
}