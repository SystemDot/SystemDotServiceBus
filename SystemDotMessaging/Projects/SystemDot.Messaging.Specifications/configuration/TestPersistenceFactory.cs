using System.Collections.Generic;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Specifications.configuration
{
    internal class TestPersistenceFactory : IPersistenceFactory
    {
        readonly Dictionary<PersistenceUseType, IPersistence> persistences;

        public TestPersistenceFactory()
        {
            this.persistences = new Dictionary<PersistenceUseType, IPersistence>();
        }

        public IPersistence CreatePersistence(PersistenceUseType useType, EndpointAddress address)
        {
            return this.persistences[useType];
        }

        public void AddPersistence(PersistenceUseType type, IPersistence toAdd)
        {
            this.persistences.Add(type, toAdd);
        }
    }
}