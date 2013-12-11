using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace SystemDot.Storage.Changes
{
    public abstract class ChangeRoot
    {
        readonly ChangeStore changeStore;
        readonly ICheckpointStrategy checkpointStrategy;
        readonly object checkPointLock = new object();
        volatile int changeCount;

        protected ChangeRoot(ChangeStore changeStore, ICheckpointStrategy checkpointStrategy)
        {
            Contract.Requires(changeStore != null);
            Contract.Requires(checkpointStrategy != null);

            this.changeStore = changeStore;
            this.checkpointStrategy = checkpointStrategy;
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
            if (ShouldCheckPoint()) UrgeCheckPoint();
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
            return checkpointStrategy.ShouldCheckPoint(changeCount);
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
                .GetMethod("ApplyChange", new[] {change.GetType()})
                .Invoke(this, new object[] {change});
        }
    }
}