using System.Diagnostics.Contracts;
using SystemDot.Messaging.MessageTransportation;

namespace SystemDot.Messaging.Channels.Messages.Distribution
{
    [ContractClass(typeof(IDistributionSubscriberContract))]
    public interface IDistributionSubscriber 
    {
        void Recieve(MessagePayload message);
    }

    [ContractClassFor(typeof(IDistributionSubscriber))]
    public class IDistributionSubscriberContract : IDistributionSubscriber
    {
        public void Recieve(MessagePayload message)
        {
            Contract.Requires(message != null);
        }
    }
}