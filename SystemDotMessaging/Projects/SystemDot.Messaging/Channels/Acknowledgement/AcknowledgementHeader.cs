using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Acknowledgement
{
    public class AcknowledgementHeader : IMessageHeader
    {
        public AcknowledgementHeader()
        {
        }

        public AcknowledgementHeader(MessagePersistenceId toSet)
        {
            Contract.Requires(toSet != null);
            MessageId = toSet;
        }

        public MessagePersistenceId MessageId { get; set; }

        public override string ToString()
        {
            return string.Concat(this.GetType() ,": ", MessageId.ToString());
        }
    }
}