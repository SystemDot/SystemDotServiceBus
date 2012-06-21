using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace SystemDot.Messaging.Channels.Messages.Consuming
{
    public class MessageHandlerRouter : IChannelEndPoint<object>
    {
        readonly List<IMessageHandler> handlers;

        public MessageHandlerRouter()
        {
            this.handlers = new List<IMessageHandler>();
        }

        public void InputMessage(object toInput)
        {
            RouteMessageToHandlers(toInput);
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