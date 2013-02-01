using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Transport.InProcess
{
    public class MessageReciever : IMessageReciever
    {      
        public event Action<MessagePayload> MessageProcessed;

        public MessageReciever(InProcessMessageServer server)
        {
            server.MessageProcessed += payload => MessageProcessed(payload);
        }

        public void RegisterAddress(EndpointAddress toRegister)
        {
            Contract.Requires(toRegister != EndpointAddress.Empty);
        }
    }
}