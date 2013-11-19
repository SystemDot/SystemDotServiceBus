using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Serialisation;

namespace SystemDot.Storage.Changes
{
    public abstract class ChangeStore : Disposable, IChangeStore
    {
        readonly ISerialiser serialiser;

        protected ChangeStore(ISerialiser serialiser)
        {
            Contract.Requires(serialiser != null);
            this.serialiser = serialiser;
        }

        public abstract void Initialise();

        public void StoreChange(string changeRootId, Change change)
        {
            StoreChange(changeRootId, change, SerialiseChange);
        }

        protected abstract void StoreChange(string changeRootId, Change change, Func<Change, byte[]> serialiseAction);

        byte[] SerialiseChange(Change toDeserialise)
        {
            return serialiser.Serialise(toDeserialise);
        }

        public IEnumerable<Change> GetChanges(string changeRootId)
        {
            return GetChanges(changeRootId, DeserialiseChange);
        }

        Change DeserialiseChange(byte[] toDeserialise)
        {
            return serialiser
                .Deserialise(toDeserialise)
                .As<Change>();
        }

        protected abstract IEnumerable<Change> GetChanges(string changeRootId, Func<byte[], Change> deserialiseAction);
    }
}