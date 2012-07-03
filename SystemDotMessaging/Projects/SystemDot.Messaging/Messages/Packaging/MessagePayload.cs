using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Messages.Packaging
{
    [Serializable]
    public class MessagePayload
    {
        readonly List<IMessageHeader> headers;

        public IEnumerable<IMessageHeader> Headers 
        { 
            get
            {
                return this.headers;
            }
        }

        public MessagePayload()
        {
            this.headers = new List<IMessageHeader>();
        }

        public void AddHeader(IMessageHeader toAdd)
        {
            Contract.Requires(toAdd != null);

            this.headers.Add(toAdd);
        }
    }
}