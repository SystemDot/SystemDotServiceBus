using System;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Messages.Packaging.Headers
{
    public class BodyHeader : IMessageHeader
    {
        public byte[] Body { get; set; }

        public BodyHeader() {}

        public BodyHeader(byte[] body)
        {
            Contract.Requires(body != null);
            Body = body;
        }
    }
}