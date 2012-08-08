using System;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Channels.RequestReply
{
    public interface ISubscriptionRequestor : IMessageProcessor<MessagePayload>
    {
        void Start();
    }
}