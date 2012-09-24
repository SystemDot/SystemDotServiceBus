using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Channels.Packaging
{
    public class MessagePayload
    {
        public List<IMessageHeader> Headers { get; set; }

        public Guid Id { get; set; }

        public MessagePayload()
        {
            Headers = new List<IMessageHeader>();
            Id = Guid.NewGuid();
        }

        public void AddHeader(IMessageHeader toAdd)
        {
            Contract.Requires(toAdd != null);

            this.Headers.Add(toAdd);
        }
    }
}