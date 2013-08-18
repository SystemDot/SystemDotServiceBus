using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Hooks
{
    [ContractClass(typeof (IMessageHookContract<>))]
    public interface IMessageHook<T>
    {
        void ProcessMessage(T toInput, Action<T> toPerformOnOutput);
    }

    [ContractClassFor(typeof(IMessageHook<>))]
    public abstract class IMessageHookContract<T> : IMessageHook<T>
    {
        public void ProcessMessage(T toInput, Action<T> toPerformOnOutput)
        {
            Contract.Requires(!toInput.Equals(default(T)));
            Contract.Requires(toPerformOnOutput != null);
        }
    }
}