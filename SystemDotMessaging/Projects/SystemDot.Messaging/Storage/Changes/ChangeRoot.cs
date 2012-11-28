using System;
using System.Reflection;

namespace SystemDot.Messaging.Storage.Changes
{
    public class ChangeRoot
    {
        readonly IChangeStore changeStore;
        bool hasChanged;

        protected ChangeRoot(IChangeStore changeStore)
        {
            this.changeStore = changeStore;
        }

        protected string Id { get; set; }

        public void Initialise()
        {
            this.changeStore.GetChanges(Id).ForEach(ReplayChange);
        }

        public bool HasChanged()
        {
            return this.hasChanged;
        }


        protected void AddChange(Change change)
        {
            this.changeStore.StoreChange(Id, change);
            ReplayChange(change);   
        }

        void ReplayChange(Change change)
        {
            this.hasChanged = true;
            
            GetType()
                .GetMethod("ApplyChange", new[] { change.GetType() })
                .Invoke(this, new object[] { change });
        }
    }
}