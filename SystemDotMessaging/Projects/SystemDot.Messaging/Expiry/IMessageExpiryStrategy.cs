using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Expiry
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