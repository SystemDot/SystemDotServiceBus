using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Storage.Changes
{
    public class InMemoryChangeStore : IChangeStore
    {
        readonly ConcurrentDictionary<int, ChangeContainer> changes;
        readonly ISerialiser serialiser;
        int sequence;

        public InMemoryChangeStore(ISerialiser serialiser)
        {
            this.serialiser = serialiser;
            this.changes = new ConcurrentDictionary<int, ChangeContainer>();
        }

        public void Initialise(string connection)
        {            
        }

        public void StoreChange(string changeRootId, Change change)
        {
            var changeContainer = CreateContainer(changeRootId, change);
            this.changes.TryAdd(changeContainer.Sequence, changeContainer);
        }

        ChangeContainer CreateContainer(string changeRootId, Change change)
        {
            return new ChangeContainer(this.sequence++, changeRootId, this.serialiser.Serialise(change));
        }

        public IEnumerable<Change> GetChanges(string changeRootId)
        {
            return this.changes.Values
                .Where(c => c.ChangeRootId == changeRootId)
                .Select(DerserialiseChange);
        }

        public void CheckPoint(string changeRootId, Change change)
        {
            var changeContainer = CreateContainer(changeRootId, change);
            this.changes.TryAdd(changeContainer.Sequence, changeContainer);

           
        }

        Change DerserialiseChange(ChangeContainer container)
        {
            return this.serialiser.Deserialise(container.Change).As<Change>();
        }
    }
}