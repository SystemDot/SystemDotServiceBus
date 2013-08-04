using System.Collections.Generic;

namespace SystemDot.Storage.Changes
{
    public interface IChangeStore
    {
        void Initialise();
        void StoreChange(string changeRootId, Change change);
        IEnumerable<Change> GetChanges(string changeRootId);
    }
}