using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Messages.Processing;

namespace SystemDot.Messaging
{
    public class MessageBus : IBus
    {
        readonly ReplyChannelLookup replyChannelLookup;

        public MessageBus(ReplyChannelLookup replyChannelLookup)
        {
            this.replyChannelLookup = replyChannelLookup;
        }

        public event Action<object> MessageSent;
        public event Action<object> MessagePublished;

        public void Send(object message)
        {
            Contract.Requires(message != null);
            MessageSent(message);
        }

        public void Reply(object message)
        {
            Contract.Requires(message != null);
            this.replyChannelLookup.GetCurrentChannel().InputMessage(message);
        }

        public void Publish(object message)
        {
            Contract.Requires(message != null);
            MessagePublished(message);
        }
    }
}