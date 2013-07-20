using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

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
            IEnumerator<IMessageHook<T>> hooksEnumerator = hooks.GetEnumerator();
            hooksEnumerator.MoveNext();

            if (hooksEnumerator.Current != null)
                RunHook(toInput, hooksEnumerator);
            else
                MessageProcessed(toInput);
        }

        private void RunHook(T toInput, IEnumerator<IMessageHook<T>> hooksEnumerator)
        {
            IMessageHook<T> hook = hooksEnumerator.Current;
            hooksEnumerator.MoveNext();

            if(hooksEnumerator.Current != null)
                hook.ProcessMessage(toInput, m => RunHook(m, hooksEnumerator));
            else
                hook.ProcessMessage(toInput, m => MessageProcessed(m));
        }

        public event Action<T> MessageProcessed;
    }
}