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
            
            StorageFile file;

            try
            {
                file = await GetFile(fileName);
            }
            catch (FileNotFoundException)
            {
                return;
            }

            this.document = await XmlDocument.LoadFromFileAsync(file).AsTask().ConfigureAwait(false);
        }

        static async Task<StorageFile> GetFile(string fileName)
        {
            return await Windows.ApplicationModel.Package.Current.InstalledLocation.GetFileAsync(fileName);
        }

        public IEnumerable<XmlNode> GetSettingsInSection(string section)
        {
            Contract.Assert(!string.IsNullOrEmpty(section));

            if (this.document == null) return new List<XmlNode>();
                
            return this.document.SelectSingleNode(section).ChildNodes.Select(n => new XmlNode(n));
        }
    }
}