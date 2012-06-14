using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using SystemDot.Pipes;

namespace SystemDot.Messaging.Recieving
{
    public class MessageHandlerRouter
    {
        readonly List<IMessageHandler> handlers;

        public MessageHandlerRouter(IPipe<object> pipe)
        {
            Contract.Requires(pipe != null);
            
            this.handlers = new List<IMessageHandler>();
            pipe.ItemPushed += RouteMessageToHandlers;
        }

        void RouteMessageToHandlers(object message)
        {
            this.handlers.ForEach(c =>
            {
                if (GetHandlerTypeForMessageType(message.GetType()).IsInstanceOfType(c))
                    Invoke(c, message);
            });
        }

        Type GetHandlerTypeForMessageType(Type messageType)
        {
            return typeof(IMessageHandler<>).MakeGenericType(messageType);
        }

        static void Invoke(IMessageHandler handler, object message)
        {
            GetConsumerMethodInfo(handler, message).Invoke(handler, new[] { message });
        }

        static MethodInfo GetConsumerMethodInfo(IMessageHandler consumer, object message)
        {
            return consumer.GetType().GetMethod("Handle", new [] { message.GetType() });
        }

        public void RegisterHandler(IMessageHandler toRegister)
        {
            Contract.Requires(toRegister != null);

            this.handlers.Add(toRegister);
        }
    }
}