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

        public MessageHandlerList GetHandlers<TMessage>()
        {
            return this.handlers[typeof(TMessage)];
        }

        public bool ContainsHandler<TMessage>()
        {
            return this.handlers.ContainsKey(typeof(TMessage));
        }

        public void Register<TMessage>(Action<TMessage> toRegister)
        {
            Contract.Requires(toRegister != null);

            if(!ContainsHandler<TMessage>())
                this.handlers.TryAdd(typeof(TMessage), new MessageHandlerList());

            this.handlers[typeof(TMessage)].TryAdd(toRegister, toRegister);
        }

        public void Clear()
        {
            this.handlers.Clear();
        }

        public void Unregister<TMessage>(Action<TMessage> toUnregister)
        {
            Contract.Requires(toUnregister != null);

            if (!this.handlers.ContainsKey(typeof(TMessage))) return;

            object outValue;
            this.handlers[typeof(TMessage)].TryRemove(toUnregister, out outValue);
        }
    }
}