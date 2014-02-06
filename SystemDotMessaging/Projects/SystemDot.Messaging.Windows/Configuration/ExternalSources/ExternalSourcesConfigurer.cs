using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using SystemDot.Core;
using SystemDot.Core.Collections;

namespace SystemDot.Messaging.Configuration.ExternalSources
{
    class ExternalSourcesConfigurer : IExternalSourcesConfigurer
    {
        [ImportMany]
        public IEnumerable<IExternalConfigurationSource> Sources { get; set; }

        public ExternalSourcesConfigurer()
        {
            Sources = new List<IExternalConfigurationSource>();
        }

        public void Configure(MessagingConfiguration toConfigureAgainst, MessageServerConfiguration serverToConfigureAgainst)
        {
            var catalog = new DirectoryCatalog(GetPath(), "*.ExternalConfiguration.dll");
            var container = new CompositionContainer(catalog);

            container.ComposeParts(this);
            this.Sources.ForEach(s => s.Configure(toConfigureAgainst, serverToConfigureAgainst));
        }

        static string GetPath()
        {
            return Path.GetDirectoryName(typeof(IExternalConfigurationSource).GetAssembly().Location);
        }
    }
}