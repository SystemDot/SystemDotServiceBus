using System;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Specifications
{
    public class TestMessageReciever : IMessageReciever
    {
        EndpointAddress pollingAddress;

        public event Action<MessagePayload> MessageProcessed;

        public void RecieveMessage(MessagePayload toRecieve)
        {
            if (toRecieve.GetToAddress() != this.pollingAddress) return;
            MessageProcessed(toRecieve);
        }

        public void StartPolling(EndpointAddress address)
        {
            this.pollingAddress = address;
        }
    }
}