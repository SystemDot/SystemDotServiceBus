using System;
using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public interface IReplyChannelBuilder
    {
        void Build(Guid channelIdentifier, EndpointAddress recieverAddress);
    }
}