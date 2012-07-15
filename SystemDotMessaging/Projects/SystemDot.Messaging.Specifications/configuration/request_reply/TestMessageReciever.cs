using System;
using System.Collections.Generic;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Specifications.configuration.request_reply
{
    public class TestMessageReciever : IMessageReciever
    {
        public List<EndpointAddress> ListeningAddresses { get; private set; }

        public TestMessageReciever()
        {
            this.ListeningAddresses = new List<EndpointAddress>();
        }

        public event Action<MessagePayload> MessageProcessed;
        public void RegisterListeningAddress(EndpointAddress toRegister)
        {
            this.ListeningAddresses.Add(toRegister);
        }
    }
}