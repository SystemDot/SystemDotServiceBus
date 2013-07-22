using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace SystemDot.Messaging.Hooks
{
    class MessageHookRunner<T> : IMessageProcessor<T, T>
    {
        readonly IEnumerable<IMessageHook<T>> hooks;

        public MessageHookRunner(IEnumerable<IMessageHook<T>> hooks)
        {
            Contract.Requires(hooks != null);
            this.hooks = hooks;
        }

        public void InputMessage(T toInput)
        {
            ProcessMessage(toInput, 0);
        }

        public void ProcessMessage(T toInput, int hookIndex)
        {
            if (hookIndex < hooks.Count())
                hooks.ElementAt(hookIndex).ProcessMessage(toInput, m => ProcessMessage(m, hookIndex + 1));
            else
                MessageProcessed(toInput);
        }

        public event Action<T> MessageProcessed;
    }
}