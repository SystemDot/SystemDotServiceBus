using System.Collections.Generic;
using SystemDot.Messaging.Messages;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public interface IRecieveChannelBuilder
    {
        void Build();
        void Build(params IMessageProcessor<object, object>[] hooks);
    }
}