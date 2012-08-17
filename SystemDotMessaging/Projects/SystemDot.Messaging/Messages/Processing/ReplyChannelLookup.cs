using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Threading;

namespace SystemDot.Messaging.Messages.Processing
{
    public class ReplyChannelLookup
    {
        readonly ThreadLocal<EndpointAddress> currentChannel;
        
        public ReplyChannelLookup()
        {
            this.currentChannel = new ThreadLocal<EndpointAddress>();
        }

        public void SetCurrentChannel(EndpointAddress toSet)
        {
            Contract.Requires(toSet != EndpointAddress.Empty);

            this.currentChannel.Value = toSet;
        }

        public EndpointAddress GetCurrentChannel()
        {
            return this.currentChannel.Value;
        }
    }
}