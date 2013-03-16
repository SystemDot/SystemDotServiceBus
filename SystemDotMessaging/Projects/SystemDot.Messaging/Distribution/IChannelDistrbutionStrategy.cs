using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Distribution
{
    [ContractClass(typeof(IMessageExpiryStrategyContract<>))]
    interface IChannelDistrbutionStrategy<T>
    {
        void Distribute(ChannelDistributor<T> distributor, T toDistribute);
    }

    [ContractClassFor(typeof(IChannelDistrbutionStrategy<>))]
    class IMessageExpiryStrategyContract<T> : IChannelDistrbutionStrategy<T>
    {
        public void Distribute(ChannelDistributor<T> distributor, T toDistribute)
        {
            Contract.Requires(distributor != null);
            Contract.Requires(!toDistribute.Equals(default(T)));
        }
    }
}