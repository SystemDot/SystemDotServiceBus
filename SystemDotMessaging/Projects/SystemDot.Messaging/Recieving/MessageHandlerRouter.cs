using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using SystemDot.Messaging.Pipes;

namespace SystemDot.Messaging.Recieving
{
    public class MessageHandlerRouter
    {
        readonly List<IMessageHandler> handlers;

        public MessageHandlerRouter(IPipe pipe)
        {
            Contract.Requires(pipe != null);
            
            this.handlers = new List<IMessageHandler>();
            pipe.MessagePublished += BroadcastMessageToConsumers;
        }

        void BroadcastMessageToConsumers(object message)
        {
            this.handlers.ForEach(c =>
            {
                if (GetConsumerTypeForMessageType(message.GetType()).IsInstanceOfType(c))
                    Invoke(c, message);
            });
        }

        Type GetConsumerTypeForMessageType(Type messageType)
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