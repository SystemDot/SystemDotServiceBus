using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.MessageTransportation
{
    [Serializable]
    public class MessagePayload
    {
        readonly List<IMessageHeader> headers;

        public string Address { get; private set; }

        public IEnumerable<IMessageHeader> Headers 
        { 
            get
            {
                return this.headers;
            }
        }
        
        public MessagePayload(string address)
        {
            Contract.Requires(!string.IsNullOrEmpty(address));

            Address = address;
            this.headers = new List<IMessageHeader>();
        }

        public void AddHeader(IMessageHeader toAdd)
        {
            Contract.Requires(toAdd != null);

            this.headers.Add(toAdd);
        }
    }
}