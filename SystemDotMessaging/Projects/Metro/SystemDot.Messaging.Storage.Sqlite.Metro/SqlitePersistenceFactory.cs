using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Storage.Sqlite.Metro
{
    public class SqlitePersistenceFactory : IPersistenceFactory
    {
        readonly ISerialiser serialiser;

        public SqlitePersistenceFactory(ISerialiser serialiser)
        {
            this.serialiser = serialiser;
        }

        public IPersistence CreatePersistence(PersistenceUseType useType, EndpointAddress address)
        {
            return new SqlitePersistence(this.serialiser, useType, address);
        }
    }
}