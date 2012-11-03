namespace SystemDot.Messaging.Storage
{
    public class SourcePersistenceHeader : PersistenceHeader
    {
        public SourcePersistenceHeader()
        {
        }

        public SourcePersistenceHeader(MessagePersistenceId id) : base(id)
        {
        }
    }
}