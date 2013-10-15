using SystemDot.Messaging.Builders;
using SystemDot.Messaging.ExceptionHandling;

namespace SystemDot.Messaging.RequestReply.Builders
{
    class RequestRecieveChannelSchema : RecieveChannelSchema
    {
        public bool HandleRequestsOnMainThread { get; set; }
        public bool ContinueOnException { get; set; }
    }
}