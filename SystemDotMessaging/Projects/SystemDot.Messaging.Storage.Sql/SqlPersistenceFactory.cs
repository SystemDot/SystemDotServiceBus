using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Storage.Sql
{
    public class SqlPersistenceFactory : IPersistenceFactory
    {
        private readonly ISerialiser serialiser;

        public SqlPersistenceFactory(ISerialiser serialiser)
        {
            this.serialiser = serialiser;
        }

        public IPersistence CreatePersistence(PersistenceUseType useType, EndpointAddress address)
        {
            return new SqlPersistence(this.serialiser, useType, address);
        }
    }
}