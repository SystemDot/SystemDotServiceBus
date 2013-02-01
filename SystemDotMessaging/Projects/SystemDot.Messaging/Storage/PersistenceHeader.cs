using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Storage
{
    public class PersistenceHeader : IMessageHeader
    {
        public MessagePersistenceId PersistenceId { get; set; }

        public PersistenceHeader()
        {
        }

        public PersistenceHeader(MessagePersistenceId messagePersistenceId)
        {
            PersistenceId = messagePersistenceId;
        }

        public override string ToString()
        {
            return string.Concat(this.GetType() ,": ", PersistenceId.ToString());
        }
    }
}