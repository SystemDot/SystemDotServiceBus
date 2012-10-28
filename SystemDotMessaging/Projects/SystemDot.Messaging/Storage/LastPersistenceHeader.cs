namespace SystemDot.Messaging.Storage
{
    public class LastPersistenceHeader : PersistenceHeader
    {
        public LastPersistenceHeader()
        {
        }

        public LastPersistenceHeader(MessagePersistenceId id) : base(id)
        {
        }
    }
}