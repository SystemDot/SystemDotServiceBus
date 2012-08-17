using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;

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