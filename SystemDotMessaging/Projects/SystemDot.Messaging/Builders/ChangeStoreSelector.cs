using System.Diagnostics.Contracts;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Builders
{
    class ChangeStoreSelector
    {
        readonly ChangeStore durableChangeStore;
        readonly NullChangeStore nullChangeStore;

        public ChangeStoreSelector(ChangeStore durableChangeStore, NullChangeStore nullChangeStore)
        {
            Contract.Requires(durableChangeStore != null);
            Contract.Requires(nullChangeStore != null);

            this.durableChangeStore = durableChangeStore;
            this.nullChangeStore = nullChangeStore;
        }

        public ChangeStore SelectChangeStore(IDurableOptionSchema schema)
        {
            return schema.IsDurable ? durableChangeStore : nullChangeStore;
        }
    }
}