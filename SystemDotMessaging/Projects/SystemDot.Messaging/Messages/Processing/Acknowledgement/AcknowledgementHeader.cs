using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Messages.Processing.Acknowledgement
{
    public class AcknowledgementHeader : IMessageHeader
    {
        public AcknowledgementHeader()
        {
        }

        public AcknowledgementHeader(Guid toSet)
        {
            Contract.Requires(toSet != Guid.Empty);
            MessageId = toSet;
        }

        public Guid MessageId { get; set; }
    }
}