using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Transport.Http.Remote
{
    public class LongPollRequestHeader : IMessageHeader 
    {
        public EndpointAddress Address { get; set; }

        public LongPollRequestHeader() {}

        public LongPollRequestHeader(EndpointAddress address)
        {
            this.Address = address;
        }

        public override string ToString()
        {
            return string.Concat(this.GetType() ,": ", this.Address.ToString());
        }
    }
}