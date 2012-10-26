using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Transport.Http.LongPolling
{
    public class LongPollRequestHeader : IMessageHeader 
    {
        public EndpointAddress Address { get; set; }

        public LongPollRequestHeader() {}

        public LongPollRequestHeader(EndpointAddress address)
        {
            Address = address;
        }
    }
}