using System;

namespace SystemDot.Messaging.Specifications.channels
{
    public class TestDistributor : IDistributor
    {
        Action<object> toDistibuteTo;
        
        public bool IsRunning { get; private set; }

        public void Start(Action<object> toDistributeTo)
        {
            this.toDistibuteTo = toDistributeTo;
            IsRunning = true;
        }

        public void Distribute(object toDistribute)
        {
            if (!IsRunning) return;
            toDistibuteTo(toDistribute);
        }

        public void Stop()
        {
            IsRunning = false;
        }
    }
}