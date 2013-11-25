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
        readonly ISerialiser serialiser;

        public InMemoryChangeStore() : this(new JsonSerialiser())
        {
        }

        InMemoryChangeStore(ISerialiser serialiser) : base(serialiser, new ChangeUpcasterRunner())
        {
            this.serialiser = serialiser;
            changes = new ConcurrentDictionary<int, ChangeContainer>();
        }

        public override void Initialise()
        {
        }

        public void StoreRawChange(string changeRootId, Change change)
        {
            AddChange(changeRootId, change, SerialiseChange);
        }

        byte[] SerialiseChange(Change toSerialise)
        {
            return this.serialiser.Serialise(toSerialise);
        }

        protected override void StoreChange(string changeRootId, Change change, Func<Change, byte[]> serialiseAction)
        {
            CheckpointIfPossible(changeRootId, change);
            AddChange(changeRootId, change, serialiseAction);
        }

        void CheckpointIfPossible(string changeRootId, Change change)
        {
            if (change is CheckPointChange) CheckPointChanges(changeRootId);
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

        void AddChange(string changeRootId, Change change, Func<Change, byte[]> serialiseAction)
        {
            AddChange(CreateChangeContainer(changeRootId, change, serialiseAction));
        }

        void AddChange(ChangeContainer changeContainer)
        {
            this.changes.TryAdd(changeContainer.Sequence, changeContainer);
        }

        ChangeContainer CreateChangeContainer(string changeRootId, Change change, Func<Change, byte[]> serialiseAction)
        {
            return new ChangeContainer(sequence++, changeRootId, serialiseAction(change));
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