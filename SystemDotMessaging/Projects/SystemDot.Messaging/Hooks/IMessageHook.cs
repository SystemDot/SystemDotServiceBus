using System;

namespace SystemDot.Messaging.Hooks
{
    public interface IMessageHook<T>
    {
        void ProcessMessage(T toInput, Action<T> toPerformOnOutput);
    }
}