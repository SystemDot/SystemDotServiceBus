using System;

namespace SystemDot.Messaging
{
    public interface IDistributor
    {
        void Start(Action<object> toDistributeTo);
        void Distribute(object toDistribute);
        void Stop();
    }
}