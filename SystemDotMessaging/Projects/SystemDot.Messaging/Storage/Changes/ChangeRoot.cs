using System;
using System.Reflection;

namespace SystemDot.Messaging.Storage.Changes
{
    public class ChangeRoot
    {
        readonly IChangeStore store;

        public ChangeRoot(IChangeStore store)
        {
            this.store = store;
        }

        protected string Id { get; set; }

        public void Initialise()
        {
            this.store.GetChanges(Id).ForEach(ReplayChange);
        }

        protected void AddChange(Change change)
        {
            ReplayChange(this.store.StoreChange(Id, change));   
        }

        void ReplayChange(Guid changeId)
        {
            ReplayChange(this.store.GetChange(changeId));
        }

        void ReplayChange(Change change)
        {
            GetType()
                .GetMethod("ApplyChange", new[] { change.GetType() })
                .Invoke(this, new object[] { change });
        }
    }
}