using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Storage.Changes;

namespace SystemDot.Messaging.Channels.RequestReply
{
    public class RegisterRecieveChannelChange : Change
    {
        public EndpointAddress Address { get; set; }
    }
}