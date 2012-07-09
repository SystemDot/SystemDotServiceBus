using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Messages.Packaging
{
    [Serializable]
    public class MessagePayload
    {
        public List<IMessageHeader> Headers { get; set; }

        public MessagePayload()
        {
            this.Headers = new List<IMessageHeader>();
        }

        public void AddHeader(IMessageHeader toAdd)
        {
            Contract.Requires(toAdd != null);

            this.Headers.Add(toAdd);
        }
    }
}