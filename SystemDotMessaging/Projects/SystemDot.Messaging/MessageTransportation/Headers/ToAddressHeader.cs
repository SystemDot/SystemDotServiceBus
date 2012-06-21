using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.MessageTransportation.Headers
{
    [Serializable]
    public class ToAddressHeader : IMessageHeader 
    {
        public Address Address { get; private set; }

        public ToAddressHeader(Address address)
        {
            Contract.Requires(address != null);
            Address = address;
        }
    }
}