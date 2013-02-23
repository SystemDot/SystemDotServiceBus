using System;
using System.Threading;

namespace SystemDot.Messaging.Batching
{
    public class Batch : Disposable
    {
        static readonly ThreadLocal<Batch> current = new ThreadLocal<Batch>();
        bool completed;

        public static Batch GetCurrent()
        {
            return current.Value;
        }

        static void SetCurrent(Batch toSet)
        {
            current.Value = toSet;
        }

        public static bool HasAggregateStarted()
        {
            return GetCurrent() != null;
        }

        public event Action<bool> Finished;

        public Batch()
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
            if (this.Finished != null) this.Finished(this.completed);
        }

        public void Complete()
        {
            this.completed = true;
        }
    }
}