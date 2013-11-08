using System.Collections.Generic;

namespace SystemDot.Storage.Changes
{
    public class NullChangeStore : IChangeStore
    {
        public void Initialise()
        {            
        }

        public void StoreChange(string changeRootId, Change change)
        {
        }

        public IEnumerable<Change> GetChanges(string changeRootId)
        {
            return new List<Change>();
        }
    }
}