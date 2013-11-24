using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;
using SystemDot.Storage.Changes.Upcasting;

namespace SystemDot.Messaging.Specifications
{
    public class InMemoryChangeStore : ChangeStore
    {
        readonly ConcurrentDictionary<int, ChangeContainer> changes;
        int sequence;

        public InMemoryChangeStore(ISerialiser serialiser, ChangeUpcasterRunner changeUpcasterRunner)
            : base(serialiser, changeUpcasterRunner)
        {
            changes = new ConcurrentDictionary<int, ChangeContainer>();
        }

        public override void Initialise()
        {
        }

        protected override void StoreChange(string changeRootId, Change change, Func<Change, byte[]> serialiseAction)
        {
            if (change is CheckPointChange)
                CheckPointChanges(changeRootId);

            ChangeContainer changeContainer = CreateChangeContainer(changeRootId, change, serialiseAction);
            changes.TryAdd(changeContainer.Sequence, changeContainer);
        }

        ChangeContainer CreateChangeContainer(string changeRootId, Change change, Func<Change, byte[]> serialiseAction)
        {
            return new ChangeContainer(sequence++, changeRootId, serialiseAction(change));
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

        protected override IEnumerable<Change> GetChanges(string changeRootId, Func<byte[], Change> deserialiseAction)
        {
            return changes.Values
                .Where(c => c.ChangeRootId == changeRootId)
                .Select(c => deserialiseAction(c.Change));
        }

        class ChangeContainer
        {
            public ChangeContainer(int sequence, string id, byte[] change)
            {
                Sequence = sequence;
                ChangeRootId = id;
                Change = change;
            }

            public int Sequence { get; private set; }

            public string ChangeRootId { get; private set; }

            public byte[] Change { get; private set; }
        }
    }
}