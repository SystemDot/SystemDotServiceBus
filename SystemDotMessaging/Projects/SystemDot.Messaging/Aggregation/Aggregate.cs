using System;
using System.Threading;

namespace SystemDot.Messaging.Aggregation
{
    public class Aggregate : Disposable
    {
        static readonly ThreadLocal<Aggregate> current = new ThreadLocal<Aggregate>();

        public static Aggregate GetCurrent()
        {
            return current.Value;
        }

        static void SetCurrent(Aggregate toSet)
        {
            current.Value = toSet;
        }

        public static bool HasAggregateStarted()
        {
            return GetCurrent() != null;
        }

        public event Action Finished;

        public Aggregate()
        {
            SetCurrent(this);
        }

        protected override void DisposeOfManagedResources()
        {
            OnFinished();
            SetCurrent(null);

            base.DisposeOfManagedResources();
        }

        void OnFinished()
        {
            if (this.Finished != null) this.Finished();
        }
    }
}