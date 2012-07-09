using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Messages.Packaging.Headers
{
    public class ToAddressHeader : IMessageHeader 
    {
        public EndpointAddress Address { get; set; }

        public ToAddressHeader()
        {
        }

        public ToAddressHeader(EndpointAddress address)
        {
            Contract.Requires(address != null);
            Address = address;
        }
    }
}