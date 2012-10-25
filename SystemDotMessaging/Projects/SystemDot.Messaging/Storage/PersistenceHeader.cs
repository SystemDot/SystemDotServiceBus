using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Storage
{
    public class PersistenceHeader : IMessageHeader
    {
        public MessagePersistenceId MessagePersistenceId { get; set; }

        public PersistenceHeader()
        {
        }

        public PersistenceHeader(MessagePersistenceId messagePersistenceId)
        {
            MessagePersistenceId = messagePersistenceId;
        }
    }
}