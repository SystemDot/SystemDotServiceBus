using SystemDot.Messaging.Addressing;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.RequestReply
{
    public class RegisterRequestReceiveChannelChange : Change
    {
        public EndpointAddress Address { get; set; }
    }
}