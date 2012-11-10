using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Serialisation;
using Microsoft.Isam.Esent.Collections.Generic;

namespace SystemDot.Messaging.Storage.Esent
{
    public class EsentChangeStore : Disposable, IChangeStore
    {
        readonly ISerialiser serialiser;

        PersistentDictionary<Guid, EsentChangeContainer> changes;
        int sequence;

        public EsentChangeStore(ISerialiser serialiser)
        {
            this.serialiser = serialiser;
        }

        public void Initialise(string connection)
        {
            this.changes = new PersistentDictionary<Guid, EsentChangeContainer>(connection);

            if (this.changes.Count == 0) return;
            this.sequence = this.changes.Values.Max(c => c.Sequence);
        }

        public Guid StoreChange(string changeRootId, Change change)
        {
            var id = Guid.NewGuid();

            this.changes.Add(id, new EsentChangeContainer
            {
                ChangeRootId = changeRootId, 
                Change = SerialiseChange(change), 
                Sequence = this.sequence++
            });

            this.changes.Flush();

            return id;
        }

        public IEnumerable<Change> GetChanges(string changeRootId)
        {
            var messages = new List<Change>();

            this.changes.Values
                .Where(c => c.ChangeRootId == changeRootId)
                .OrderBy(c => c.Sequence)
                .ForEach(c => messages.Add(DeserialiseChange(c.Change)));

            return messages;
        }

        Change DeserialiseChange(string toDeserialise)
        {
            return this.serialiser
                .Deserialise(Encoding.Default.GetBytes(toDeserialise))
                .As<Change>();
        }

        string SerialiseChange(Change toSerialise)
        {
            return Encoding.Default.GetString(this.serialiser.Serialise(toSerialise));
        }

        public Change GetChange(Guid id)
        {
            return DeserialiseChange(this.changes[id].Change);
        }

        protected override void DisposeOfManagedResources()
        {
            this.changes.Dispose();
            base.DisposeOfManagedResources();
        }
    }
}