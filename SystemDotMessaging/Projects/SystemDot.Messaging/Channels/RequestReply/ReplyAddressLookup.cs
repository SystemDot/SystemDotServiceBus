using System.Diagnostics.Contracts;
using System.Threading;
using SystemDot.Messaging.Channels.Addressing;

namespace SystemDot.Messaging.Channels.RequestReply
{
    public class ReplyAddressLookup
    {
        readonly ThreadLocal<EndpointAddress> currentSenderAddress;
        readonly ThreadLocal<EndpointAddress> currentRecieverAddress;
        
        public ReplyAddressLookup()
        {
            this.currentSenderAddress = new ThreadLocal<EndpointAddress>();
            this.currentRecieverAddress = new ThreadLocal<EndpointAddress>();
        }

        public void SetCurrentSenderAddress(EndpointAddress toSet)
        {
            Contract.Requires(toSet != EndpointAddress.Empty);

            this.currentSenderAddress.Value = toSet;
        }

        public void SetCurrentRecieverAddress(EndpointAddress toSet)
        {
            Contract.Requires(toSet != EndpointAddress.Empty);

            this.currentRecieverAddress.Value = toSet;
        }

        public EndpointAddress GetCurrentSenderAddress()
        {
            return this.currentSenderAddress.Value;
        }

        public EndpointAddress GetCurrentRecieverAddress()
        {
            return this.currentRecieverAddress.Value;
        }
    }
}