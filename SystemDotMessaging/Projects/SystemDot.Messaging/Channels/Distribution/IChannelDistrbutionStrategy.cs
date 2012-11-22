using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Channels.Distribution
{
    [ContractClass(typeof(IMessageExpiryStrategyContract<>))]
    public interface IChannelDistrbutionStrategy<T>
    {
        void Distribute(ChannelDistributor<T> distributor, T toDistribute);
    }

    [ContractClassFor(typeof(IChannelDistrbutionStrategy<>))]
    public class IMessageExpiryStrategyContract<T> : IChannelDistrbutionStrategy<T>
    {
        public void Distribute(ChannelDistributor<T> distributor, T toDistribute)
        {
            Contract.Requires(distributor != null);
            Contract.Requires(!toDistribute.Equals(default(T)));
        }
    }
}