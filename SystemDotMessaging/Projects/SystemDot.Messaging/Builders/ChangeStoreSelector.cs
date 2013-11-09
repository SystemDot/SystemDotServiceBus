using System.Diagnostics.Contracts;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Builders
{
    class ChangeStoreSelector
    {
        readonly IChangeStore durableChangeStore;

        public ChangeStoreSelector(IChangeStore durableChangeStore)
        {
            Contract.Requires(durableChangeStore != null);

            this.durableChangeStore = durableChangeStore;
        }

        public IChangeStore SelectChangeStore(IDurableOptionSchema schema)
        {
            return schema.IsDurable ? durableChangeStore : new NullChangeStore();
        }
    }
}