using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.MessageTransportation
{
    [Serializable]
    public class MessagePayload
    {
        readonly List<IMessageHeader> headers;

        public Address Address { get; private set; }

        public IEnumerable<IMessageHeader> Headers 
        { 
            get
            {
                return this.headers;
            }
        }

        public MessagePayload(Address address)
        {
            Contract.Requires(address != Address.Empty);

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