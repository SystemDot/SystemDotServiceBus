using System;
using System.Collections.Generic;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Specifications
{
    public class TestMessageReciever : IMessageReciever
    {
        readonly List<EndpointAddress> pollingAddresses;

        public TestMessageReciever()
        {
            this.pollingAddresses = new List<EndpointAddress>();
        }

        public event Action<MessagePayload> MessageProcessed;

        public void RecieveMessage(MessagePayload toRecieve)
        {
            if (!this.pollingAddresses.Contains(toRecieve.GetToAddress())) 
                return;

            MessageProcessed(toRecieve);
        }

        public void StartPolling(EndpointAddress address)
        {
            this.pollingAddresses.Add(address);
        }
    }
}