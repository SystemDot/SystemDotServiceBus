using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace SystemDot.Messaging.Hooks.External
{
    public class ExternalInspectorContainer
    {
        [ImportMany]
        public IEnumerable<IExternalInspector> Hooks { get; set; }

        public ExternalInspectorContainer()
        {
            Hooks = new List<IExternalInspector>();
        }
    }
}