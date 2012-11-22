using System.Diagnostics.Contracts;
using System.Threading;
using SystemDot.Messaging.Channels.Addressing;

namespace SystemDot.Messaging.Channels.RequestReply
{
    public class ReplyAddressLookup
    {
        readonly ThreadLocal<EndpointAddress> currentSenderAddress;
        
        public ReplyAddressLookup()
        {
            this.currentSenderAddress = new ThreadLocal<EndpointAddress>();
        }

        public void SetCurrentSenderAddress(EndpointAddress toSet)
        {
            Contract.Requires(toSet != EndpointAddress.Empty);

            this.currentSenderAddress.Value = toSet;
        }

        public EndpointAddress GetCurrentSenderAddress()
        {
            return this.currentSenderAddress.Value;
        }
    }
}