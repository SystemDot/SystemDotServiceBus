using System.Diagnostics.Contracts;
using SystemDot.Messaging.MessageTransportation;

namespace SystemDot.Messaging.Channels.Messages.Distribution
{
    [ContractClass(typeof(IDistributorContract))]
    public interface IDistributor : IChannelEndPoint<MessagePayload>
    {
        void Subscribe(IDistributionSubscriber toSubscribe);
    }

    [ContractClassFor(typeof(IDistributor))]
    public class IDistributorContract : IDistributor 
    {
        public void InputMessage(MessagePayload toInput)
        {
            Contract.Requires(toInput != null);
        }

        public void Subscribe(IDistributionSubscriber toSubscribe)
        {
            Contract.Requires(toSubscribe != null);
        }
    }
}