using System;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Core.Collections;
using SystemDot.Messaging.Ioc;

namespace SystemDot.Messaging.Hooks.External
{
    public class ExternalHooker : IMessageHook<object>
    {
        readonly IEnumerable<IExternalHook> hooks;

        public static IMessageHook<object> LoadUp()
        {
            return new ExternalHooker(GetLoader().GetHooks());
        }

        static IExternalHookLoader GetLoader()
        {
            return IocContainerLocator.Locate().Resolve<IExternalHookLoader>();
        }

        ExternalHooker(IEnumerable<IExternalHook> hooks)
        {
            this.hooks = hooks;
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

        IEnumerable<IExternalHook> GetHooks(Type messageType)
        {
            return hooks.Where(h => h.MessageType == messageType );
        }
    }
}