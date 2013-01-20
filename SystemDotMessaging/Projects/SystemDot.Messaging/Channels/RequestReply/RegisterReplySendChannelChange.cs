using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Storage.Changes;

namespace SystemDot.Messaging.Channels.RequestReply
{
    public class RegisterReplySendChannelChange : Change
    {
        public EndpointAddress Address { get; set; }
    }
}