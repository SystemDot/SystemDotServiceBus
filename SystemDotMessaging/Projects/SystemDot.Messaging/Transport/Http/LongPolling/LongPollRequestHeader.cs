using System;
using System.Collections.Generic;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Transport.Http.LongPolling
{
    public class LongPollRequestHeader : IMessageHeader 
    {
        public List<EndpointAddress> Addresses { get; set; }

        public LongPollRequestHeader() {}

        public LongPollRequestHeader(List<EndpointAddress> addresses)
        {
            Addresses = addresses;
        }
    }
}