using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace SystemDot.Messaging.Hooks.External
{
    public class ExternalHookContainer
    {
        [ImportMany]
        public IEnumerable<IExternalHook> Hooks { get; set; }

        public ExternalHookContainer()
        {
            Hooks = new List<IExternalHook>();
        }
    }
}