using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Specifications
{
    public class InMemoryChangeStore : IChangeStore
    {
        readonly ConcurrentDictionary<int, ChangeContainer> changes;
        readonly ISerialiser serialiser;
        int sequence;

        public InMemoryChangeStore(ISerialiser serialiser)
        {
            this.serialiser = serialiser;
            changes = new ConcurrentDictionary<int, ChangeContainer>();
        }

        public void Initialise()
        {
        }

        public void StoreChange(string changeRootId, Change change)
        {
            if (change is CheckPointChange)
                CheckPointChanges(changeRootId);

            var changeContainer = CreateContainer(changeRootId, change);
            changes.TryAdd(changeContainer.Sequence, changeContainer);
        }

        void CheckPointChanges(string changeRootId)
        {
            ChangeContainer temp;

            changes.Values
                .Where(c => c.ChangeRootId == changeRootId)
                .Select(c => c.Sequence)
                .ToList()
                .ForEach(s => changes.TryRemove(s, out temp));
        }

        ChangeContainer CreateContainer(string changeRootId, Change change)
        {
            return new ChangeContainer(sequence++, changeRootId, serialiser.Serialise(change));
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

        class ChangeContainer
        {
            public int Sequence { get; private set; }
            public string ChangeRootId { get; private set; }
            public byte[] Change { get; private set; }

            public ChangeContainer(int sequence, string id, byte[] change)
            {
                Sequence = sequence;
                ChangeRootId = id;
                Change = change;
            }
        }
    }
}