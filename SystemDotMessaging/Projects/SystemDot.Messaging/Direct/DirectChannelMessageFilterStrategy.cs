using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Filtering;

namespace SystemDot.Messaging.Direct
{
    class DirectChannelMessageFilterStrategy : IMessageFilterStrategy
    {
        readonly EndpointAddress address;

        public DirectChannelMessageFilterStrategy(EndpointAddress address)
        {
            Contract.Requires(address != null);

            this.address = address;
        }

        public bool PassesThrough(object toCheck)
        {
            return DirectChannelMessageReplyContext.GetCurrentAddress() == address;
        }
    }
}