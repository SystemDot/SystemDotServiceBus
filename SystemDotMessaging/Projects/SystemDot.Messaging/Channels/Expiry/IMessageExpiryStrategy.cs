using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Channels.Expiry
{
    [ContractClass(typeof(IMessageExpiryStrategyContract))]
    public interface IMessageExpiryStrategy
    {
        bool HasExpired(MessagePayload toCheck);
    }

    [ContractClassFor(typeof(IMessageExpiryStrategy))]
    public class IMessageExpiryStrategyContract : IMessageExpiryStrategy
    {
        public bool HasExpired(MessagePayload toCheck)
        {
            Contract.Requires(toCheck != null);
            return false;
        }
    }
}