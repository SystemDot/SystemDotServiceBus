using System.Collections.Concurrent;
using System.Diagnostics.Contracts;
using System.Threading;

namespace SystemDot.Messaging.Messages.Processing
{
    public class ReplyChannelLookup
    {
        readonly ThreadLocal<EndpointAddress> currentChannel;
        readonly ConcurrentDictionary<EndpointAddress, IMessageProcessor<object, object>> channels;

        public ReplyChannelLookup()
        {
            this.currentChannel = new ThreadLocal<EndpointAddress>();
            this.channels = new ConcurrentDictionary<EndpointAddress, IMessageProcessor<object, object>>();
        }

        public void RegisterChannel(EndpointAddress address, IMessageProcessor<object, object> toRegister)
        {
            this.channels[address] = toRegister;
        }

        public void SetCurrentChannel(EndpointAddress toSet)
        {
            Contract.Requires(toSet != EndpointAddress.Empty);

            this.currentChannel.Value = toSet;
        }

        public IMessageProcessor<object, object> GetCurrentChannel()
        {
            return this.channels[this.currentChannel.Value];
        }
    }
}