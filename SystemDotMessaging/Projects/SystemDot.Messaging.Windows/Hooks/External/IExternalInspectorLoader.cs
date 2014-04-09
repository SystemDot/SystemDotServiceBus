using System.Collections.Generic;

namespace SystemDot.Messaging.Hooks.External
{
    public interface IExternalInspectorLoader
    {
        IEnumerable<IExternalInspector> GetHooks();
    }
}