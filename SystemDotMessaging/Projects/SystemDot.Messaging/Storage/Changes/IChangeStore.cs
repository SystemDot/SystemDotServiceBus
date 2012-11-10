using System;
using System.Collections.Generic;

namespace SystemDot.Messaging.Storage.Changes
{
    public interface IChangeStore
    {
        void Initialise(string connection);
        Guid StoreChange(string changeRootId, Change change);
        IEnumerable<Change> GetChanges(string changeRootId);
        Change GetChange(Guid id);
    }
}