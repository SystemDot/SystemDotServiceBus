using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;

namespace SystemDot.Messaging.Hooks.External
{
    public class ExternalHookLoader : IExternalHookLoader
    {       
        public IEnumerable<IExternalHook> GetHooks()
        {
            var catalog = new DirectoryCatalog(GetPath(), "*.ExternalHooks.dll");
            var container = new CompositionContainer(catalog);
            var externalHookContainer = new ExternalHookContainer();

            container.ComposeParts(externalHookContainer);

            return externalHookContainer.Hooks;
        }

        static string GetPath()
        {
            return Path.GetDirectoryName(typeof(IExternalHook).GetAssembly().GetLocation());
        }
    }
}