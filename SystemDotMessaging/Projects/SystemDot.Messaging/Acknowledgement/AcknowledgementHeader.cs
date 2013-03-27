using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Acknowledgement
{
    public class AcknowledgementHeader : IMessageHeader
    {
        public AcknowledgementHeader()
        {
        }

        public AcknowledgementHeader(MessagePersistenceId toSet)
        {
            Contract.Requires(toSet != null);
            this.MessageId = toSet;
        }

        public MessagePersistenceId MessageId { get; set; }

        public override string ToString()
        {
            return string.Concat("Message to acknowledge: ", MessageId.ToString());
        }
    }
}