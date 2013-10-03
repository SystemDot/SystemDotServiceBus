using System;
using System.Diagnostics.Contracts;

namespace SystemDot
{
    public static class Messenger
    {
        static readonly MessengerHandlerRegistry Registry = new MessengerHandlerRegistry();

        public static void Send<TMessage>(TMessage message)
        {
            Contract.Requires(!message.Equals(default(TMessage)));

            if (Registry.ContainsHandler<TMessage>())
                Registry.GetHandlers<TMessage>().ForEach(h => h.Value.As<Action<TMessage>>().Invoke(message));
        }

        public static void Register<TMessage>(Action<TMessage> toRegister)
        {
            Contract.Requires(toRegister != null);
            Registry.Register<TMessage>(toRegister);
        }

        public static void Reset()
        {
            Registry.Clear();
        }

        public static void Unregister<TMessage>(Action<TMessage> toUnregister)
        {
            Contract.Requires(toUnregister != null);
            Registry.Unregister<TMessage>(toUnregister);
        }
    }
}