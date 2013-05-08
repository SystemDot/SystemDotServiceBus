using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Repeating
{
    public class MessageRepeater : MessageProcessor
    {
        readonly IRepeatStrategy repeatStrategy;
        readonly ISystemTime systemTime;
        readonly IMessageCache messageCache;

        public MessageRepeater(IRepeatStrategy repeatStrategy, ISystemTime systemTime, IMessageCache messageCache)
        {
            Contract.Requires(repeatStrategy != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(messageCache != null);
        
            this.repeatStrategy = repeatStrategy;
            this.systemTime = systemTime;
            this.messageCache = messageCache;

            Messenger.Register<MessagingInitialising>(_ => FirstRepeat());
        }

        void FirstRepeat()
        {
            this.messageCache.GetOrderedMessages().ForEach(InputMessage);
        }

        public override void InputMessage(MessagePayload toInput)
        {
            OnMessageProcessed(toInput);
        }

        public void Repeat()
        {
            this.repeatStrategy.Repeat(this, this.messageCache, this.systemTime);
        }
    }
}