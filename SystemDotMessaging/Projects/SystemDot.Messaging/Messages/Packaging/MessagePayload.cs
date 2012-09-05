using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Messages.Packaging
{
    public class MessagePayload
    {
        public List<IMessageHeader> Headers { get; set; }

        public Guid Id { get; private set; }

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