using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;
using SystemDot.Messaging.Pipes;

namespace SystemDot.Messaging.Recieving
{
    public class ConsumerMessageBroadcaster
    {
        readonly List<IConsume> consumers;

        public ConsumerMessageBroadcaster(IPipe pipe)
        {
            Contract.Requires(pipe != null);
            
            this.consumers = new List<IConsume>();
            pipe.MessagePublished += BroadcastMessageToConsumers;
        }

        void BroadcastMessageToConsumers(object message)
        {
            this.consumers.ForEach(c =>
            {
                if (GetConsumerTypeForMessageType(message.GetType()).IsInstanceOfType(c))
                    Invoke(c, message);
            });
        }

        Type GetConsumerTypeForMessageType(Type messageType)
        {
            return typeof(IConsume<>).MakeGenericType(messageType);
        }

        static void Invoke(IConsume consumer, object message)
        {
            GetConsumerMethodInfo(consumer, message).Invoke(consumer, new[] { message });
        }

        static MethodInfo GetConsumerMethodInfo(IConsume consumer, object message)
        {
            return consumer.GetType().GetMethod("Consume", new [] { message.GetType() });
        }

        public void RegisterConsumer(IConsume toRegister)
        {
            Contract.Requires(toRegister != null);

            this.consumers.Add(toRegister);
        }
    }
}