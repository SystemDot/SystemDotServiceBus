using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;

namespace SystemDot.Messaging.Configuration.ExternalSources
{
    public class ExternalSourcesConfigurer : IExternalSourcesConfigurer
    {
        [ImportMany]
        public IEnumerable<IExternalConfigurationSource> Sources { get; set; }

        public void Configure(MessageServerConfiguration toConfigureAgainst)
        {
            var catalog = new DirectoryCatalog(GetPath(), "*.dll");
            var container = new CompositionContainer(catalog);

            container.ComposeParts(this);
            this.Sources.ForEach(s => s.Configure(toConfigureAgainst));
        }

        static string GetPath()
        {
            return Path.GetDirectoryName(typeof(IExternalConfigurationSource).GetAssembly().GetLocation());
        }
    }
}