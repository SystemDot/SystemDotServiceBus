using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Transport.InProcess
{
    public class MessageReciever : IMessageReciever
    {      
        public event Action<MessagePayload> MessageProcessed;

        public void RegisterListeningAddress(EndpointAddress toRegister)
        {
            Contract.Requires(toRegister != EndpointAddress.Empty);
            
        }
    }
}