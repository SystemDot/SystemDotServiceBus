using System.Collections.Generic;

namespace SystemDot.Messaging.Hooks.External
{
    public interface IExternalHookLoader
    {
        IEnumerable<IExternalHook> GetHooks();
    }
}