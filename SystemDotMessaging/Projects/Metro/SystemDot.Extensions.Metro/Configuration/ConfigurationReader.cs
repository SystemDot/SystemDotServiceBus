using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Storage;

namespace SystemDot.Configuration
{
    public class ConfigurationReader : IConfigurationReader
    {
        XmlDocument document;

        public void Load(string fileName)
        {
            LoadAsync(fileName).Wait();
        }

        public async Task LoadAsync(string fileName)
        {
            Contract.Assert(!string.IsNullOrEmpty(fileName));

            try
            {
                StorageFile file = await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(fileName).AsTask().ConfigureAwait(false);
                this.document = await XmlDocument.LoadFromFileAsync(file).AsTask().ConfigureAwait(false);
            }
            catch (FileNotFoundException)
            {
            }
        }

        public IEnumerable<XmlNode> GetSettingsInSection(string section)
        {
            Contract.Assert(!string.IsNullOrEmpty(section));

            if (this.document == null) return new List<XmlNode>();

            return this.document.SelectSingleNode(section).ChildNodes
                .Where(n => n.NodeType == NodeType.ElementNode)
                .Select(n => new XmlNode(n));
        }
    }
}