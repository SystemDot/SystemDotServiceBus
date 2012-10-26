using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Transport.InProcess
{
    public class MessageReciever : IMessageReciever
    {      
        public event Action<MessagePayload> MessageProcessed;

        public MessageReciever(InProcessMessageServer server)
        {
            server.MessageProcessed += payload => MessageProcessed(payload);
        }

        public void StartPolling(EndpointAddress toRegister)
        {
            Contract.Requires(toRegister != EndpointAddress.Empty);
        }
    }
}