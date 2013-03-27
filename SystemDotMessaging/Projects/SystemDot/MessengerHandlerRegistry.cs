using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace SystemDot
{
    public class MessengerHandlerRegistry
    {
        readonly Dictionary<Type, MessageHandlerList> handlers;

        public MessengerHandlerRegistry()
        {
            this.handlers = new Dictionary<Type, MessageHandlerList>();
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
                this.handlers.Add(typeof(T), new MessageHandlerList());

            this.handlers[typeof(T)].Add(toRegister);
        }

        public void Clear()
        {
            this.handlers.Clear();
        }
    }
}