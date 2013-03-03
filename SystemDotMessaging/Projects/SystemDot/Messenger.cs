using System;
using System.Diagnostics.Contracts;

namespace SystemDot
{
    public static class Messenger
    {
        static readonly MessengerHandlerRegistry Registry = new MessengerHandlerRegistry();

        public static void Send<T>(T message)
        {
            Contract.Requires(!message.Equals(default(T)));

            if (Registry.ContainsHandler<T>())
                Registry.GetHandlers<T>().ForEach(h => h.As<Action<T>>().Invoke(message));
        }

        public static void Register<T>(Action<T> toRegister)
        {
            Contract.Requires(toRegister != null);
            Registry.Register<T>(toRegister);
        }

        public static void Reset()
        {
            Registry.Clear();
        }
    }
}