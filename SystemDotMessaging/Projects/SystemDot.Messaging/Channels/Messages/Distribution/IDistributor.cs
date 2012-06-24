using System.Diagnostics.Contracts;
using SystemDot.Messaging.MessageTransportation;

namespace SystemDot.Messaging.Channels.Messages.Distribution
{
    [ContractClass(typeof(IDistributorContract))]
    public interface IDistributor : IMessageInputter<MessagePayload>
    {
        void Subscribe(IMessageInputter<MessagePayload> toSubscribe);
    }

    [ContractClassFor(typeof(IDistributor))]
    public class IDistributorContract : IDistributor 
    {
        public void InputMessage(MessagePayload toInput)
        {
            Contract.Requires(toInput != null);
        }

        public void Subscribe(IMessageInputter<MessagePayload> toSubscribe)
        {
            Contract.Requires(toSubscribe != null);
        }
    }
}