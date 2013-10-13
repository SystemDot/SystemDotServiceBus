using System.Collections.Generic;
using System.Linq;

namespace SystemDot.Storage.Changes
{
    public abstract class ChangeRoot
    {
        readonly IChangeStore changeStore;
        readonly object checkPointLock = new object();
        volatile int changeCount;

        protected ChangeRoot(IChangeStore changeStore)
        {
            this.changeStore = changeStore;
        }

        protected string Id { get; set; }

        public virtual void Initialise()
        {
            List<Change> changes = changeStore.GetChanges(Id).ToList();
            changeCount = changes.Count;

            changes.ForEach(ReplayChange);
        }

        protected void AddChange(Change change)
        {
            if (ShouldCheckPoint())
                UrgeCheckPoint();

            AddChangeWithoutCheckPoint(change);
        }

        protected abstract void UrgeCheckPoint();

        protected void CheckPoint(CheckPointChange change)
        {
            lock (checkPointLock)
            {
                if (!ShouldCheckPoint()) return;
                
                AddChangeWithoutCheckPoint(change);
                
                changeCount = 0;
            }
        }

        bool ShouldCheckPoint()
        {
            return changeCount >= 1000;
        }

        void AddChangeWithoutCheckPoint(Change change)
        {
            changeStore.StoreChange(Id, change);
            ReplayChange(change);

            changeCount++;
        }

        void ReplayChange(Change change)
        {
            GetType()
                .GetMethod("ApplyChange", new[] { change.GetType() })
                .Invoke(this, new object[] { change });
        }
    }
}