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
            if (change is CheckPointChange) 
                CheckPointChanges(changeRootId);

            var changeContainer = CreateContainer(changeRootId, change);
            this.changes.TryAdd(changeContainer.Sequence, changeContainer);
        }

        void CheckPointChanges(string changeRootId)
        {
            ChangeContainer temp;

            this.changes.Values
                .Where(c => c.ChangeRootId == changeRootId)
                .Select(c => c.Sequence)
                .ToList()
                .ForEach(s => this.changes.TryRemove(s, out temp));
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

        Change DerserialiseChange(ChangeContainer container)
        {
            return this.serialiser.Deserialise(container.Change).As<Change>();
        }
    }
}