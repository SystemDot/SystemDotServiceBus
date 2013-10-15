using SystemDot.Messaging.Builders;

namespace SystemDot.Messaging.RequestReply.Builders
{
    class ReplyReceiveChannelSchema : RecieveChannelSchema
    {
        public bool HandleRepliesOnMainThread { get; set; }
        public bool ContinueOnException { get; set; }
    }
}