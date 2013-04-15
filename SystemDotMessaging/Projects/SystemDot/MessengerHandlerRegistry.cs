using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;

namespace SystemDot
{
    public class MessengerHandlerRegistry
    {
        readonly ConcurrentDictionary<Type, MessageHandlerList> handlers;

        public MessengerHandlerRegistry()
        {
            this.handlers = new ConcurrentDictionary<Type, MessageHandlerList>();
        }

        public MessageHandlerList GetHandlers<T>()
        {
            return this.handlers[typeof (T)];
        }

        public bool ContainsHandler<T>()
        {
            return this.handlers.ContainsKey(typeof(T));
        }

        public void Register<T>(Action<T> toRegister)
        {
            Contract.Requires(toRegister != null);

            if(!ContainsHandler<T>())
                this.handlers.TryAdd(typeof(T), new MessageHandlerList());

            this.handlers[typeof(T)].TryAdd(toRegister, toRegister);
        }

        public void Clear()
        {
            this.handlers.Clear();
        }
    }
}