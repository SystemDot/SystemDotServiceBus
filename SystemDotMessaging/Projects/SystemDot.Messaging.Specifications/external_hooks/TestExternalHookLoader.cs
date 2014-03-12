using System.Collections.Generic;
using SystemDot.Messaging.Hooks.External;

namespace SystemDot.Messaging.Specifications.external_hooks
{
    public class TestExternalHookLoader : IExternalHookLoader
    {
        readonly List<IExternalHook> hooks;

        public TestExternalHookLoader()
        {
            hooks = new List<IExternalHook>();
        }

        public void AddHook(IExternalHook hook)
        {
            hooks.Add(hook);
        }

        public IEnumerable<IExternalHook> GetHooks()
        {
            return hooks;
        }
    }
}