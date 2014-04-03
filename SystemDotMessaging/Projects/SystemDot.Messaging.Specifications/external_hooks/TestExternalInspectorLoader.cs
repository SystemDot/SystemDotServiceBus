using System.Collections.Generic;
using SystemDot.Messaging.Hooks.External;

namespace SystemDot.Messaging.Specifications.external_hooks
{
    public class TestExternalInspectorLoader : IExternalInspectorLoader
    {
        readonly List<IExternalInspector> hooks;

        public TestExternalInspectorLoader()
        {
            hooks = new List<IExternalInspector>();
        }

        public void AddHook(IExternalInspector inspector)
        {
            hooks.Add(inspector);
        }

        public IEnumerable<IExternalInspector> GetHooks()
        {
            return hooks;
        }
    }
}