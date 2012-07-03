using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Messages.Packaging.Headers
{
    [Serializable]
    public class ToAddressHeader : IMessageHeader 
    {
        public EndpointAddress Address { get; private set; }

        public ToAddressHeader(EndpointAddress address)
        {
            Contract.Requires(address != null);
            Address = address;
        }
    }
}