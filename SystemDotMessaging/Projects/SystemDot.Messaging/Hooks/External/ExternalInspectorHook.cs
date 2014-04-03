using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Messaging.Ioc;

namespace SystemDot.Messaging.Hooks.External
{
    public class ExternalInspectorHook : IMessageHook<object>
    {
        readonly IEnumerable<IExternalInspector> hooks;

        public static IMessageHook<object> LoadUp()
        {
            return IocContainerLocator.Locate().Resolve<ExternalInspectorHook>();
        }

        public ExternalInspectorHook(IExternalInspectorLoader loader)
        {
            hooks = loader.GetHooks();
        }

        public void ProcessMessage(object toInput, Action<object> toPerformOnOutput)
        {
            toPerformOnOutput(RunThroughExternalHooks(toInput));
        }

        object RunThroughExternalHooks(object toInput)
        {
            GetHooks(toInput.GetType()).ForEach(h => toInput = h.ProcessMessage(toInput));
            return toInput;
        }

        IEnumerable<IExternalInspector> GetHooks(Type messageType)
        {
            return hooks.Where(h => h.MessageType == messageType );
        }
    }
}