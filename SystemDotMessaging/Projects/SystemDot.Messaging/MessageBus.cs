using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging
{
    public class MessageBus : IMessageProcessor<object>
    {
        static MessageBus instance;

        public static void Send(object message)
        {
            Contract.Requires(message != null);

            instance.SendMessage(message);
        }

        public event Action<object> MessageProcessed;

        public MessageBus()
        {
            instance = this;
        }

        void SendMessage(object message)
        {
            MessageProcessed(message);
        }        
    }
}