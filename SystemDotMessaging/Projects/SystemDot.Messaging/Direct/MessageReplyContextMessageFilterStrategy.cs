using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Filtering;

namespace SystemDot.Messaging.Direct
{
    class MessageReplyContextMessageFilterStrategy : IMessageFilterStrategy
    {
        readonly EndpointAddress address;

        public MessageReplyContextMessageFilterStrategy(EndpointAddress address)
        {
            Contract.Requires(address != null);

            this.address = address;
        }

        public bool PassesThrough(object toCheck)
        {
            return MessageReplyContext.GetCurrentAddress() == address;
        }
    }
}