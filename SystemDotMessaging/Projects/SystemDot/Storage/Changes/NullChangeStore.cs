using System;
using System.Collections.Generic;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes.Upcasting;

namespace SystemDot.Storage.Changes
{
    public class NullChangeStore : ChangeStore
    {
        public NullChangeStore(ISerialiser serialiser, ChangeUpcasterRunner upcasterRunner) 
            : base(serialiser, upcasterRunner)
        {
        }

        public override void Initialise()
        {            
        }

        protected override void StoreChange(string changeRootId, Change change, Func<Change, byte[]> serialiseAction)
        {
        }

        protected override IEnumerable<Change> GetChanges(string changeRootId, Func<byte[], Change> deserialiseAction)
        {
            return new List<Change>();
        }

        protected override IEnumerable<ChangeDescription> GetChangeDescriptions(
            Func<string, long, byte[], ChangeDescription> descriptionCreator)
        {
            return new List<ChangeDescription>();
        }
    }
}