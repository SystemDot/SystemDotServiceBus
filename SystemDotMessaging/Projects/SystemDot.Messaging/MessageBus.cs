using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels;

namespace SystemDot.Messaging
{
    public class MessageBus : IChannelStartPoint<object>
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