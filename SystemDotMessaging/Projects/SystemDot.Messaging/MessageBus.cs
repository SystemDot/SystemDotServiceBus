using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging
{
    public class MessageBus : IBus
    {
        public event Action<object> MessageProcessed;

        public void Send(object message)
        {
            Contract.Requires(message != null);
            MessageProcessed(message);
        }        
    }
}