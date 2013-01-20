using System;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Channels.Errors
{
    public class ErrorResender : IMessageProcessor<MessagePayload>
    {
        readonly MessageCache messageCache;
        
        public ErrorResender(MessageCache messageCache)
        {
            this.messageCache = messageCache;
        }

        public event Action<MessagePayload> MessageProcessed;

        public void ResendAllMessages()
        {
            this.messageCache.GetMessages().ForEach(this.MessageProcessed);
        }
    }
}