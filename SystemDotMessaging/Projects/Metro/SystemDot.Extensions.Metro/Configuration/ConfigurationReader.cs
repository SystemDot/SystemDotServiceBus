using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.Foundation;
using Windows.Storage;

namespace SystemDot.Configuration
{
    public class ConfigurationReader
    {
        XmlDocument document;

        public void Load(string fileName)
        {
            LoadAsync(fileName).Wait();
        }

        public async Task LoadAsync(string fileName)
        {
            Contract.Assert(!string.IsNullOrEmpty(fileName));
            
            StorageFile file;

            try
            {
                file = await GetFile(fileName);
            }
            catch (FileNotFoundException)
            {
                return;
            }

            this.document = XmlDocument.LoadFromFileAsync(file).GetAwaiter().GetResult();
        }

        static IAsyncOperation<StorageFile> GetFile(string fileName)
        {
            return Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(fileName);
        }

        public Dictionary<string, string> GetSettingsInSection(string section)
        {
            Contract.Assert(!string.IsNullOrEmpty(section));

            return GetSettingNodesInSection(section)
                .Where(node => node.NodeType == NodeType.ElementNode)
                .ToDictionary(node => node.NodeName, node => node.InnerText);
        }

        IEnumerable<IXmlNode> GetSettingNodesInSection(string section)
        {
            if (this.document == null) 
                return new List<IXmlNode>();
                
            return this.document.SelectSingleNode(section).ChildNodes;
        }
    }
}