using System;
using System.Collections.Generic;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Transport.Http.LongPolling
{
    [Serializable]
    public class LongPollRequestHeader : IMessageHeader 
    {
        public List<EndpointAddress> Addresses { get; private set; }

        public LongPollRequestHeader(List<EndpointAddress> addresses)
        {
            Addresses = addresses;
        }
    }
}