using System.Diagnostics.Contracts;
using SystemDot.Core;
using SystemDot.Core.Collections;
using SystemDot.Messaging.Handling.Actions;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Simple;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Repeating
{
    public class MessageRepeater : MessageProcessor
    {
        readonly IRepeatStrategy repeatStrategy;
        readonly ISystemTime systemTime;
        readonly IMessageCache messageCache;
        ActionSubscriptionToken<MessagingInitialising> token;

        public MessageRepeater(IRepeatStrategy repeatStrategy, ISystemTime systemTime, IMessageCache messageCache)
        {
            Contract.Requires(repeatStrategy != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(messageCache != null);
        
            this.repeatStrategy = repeatStrategy;
            this.systemTime = systemTime;
            this.messageCache = messageCache;

            token = Messenger.RegisterHandler<MessagingInitialising>(_ => FirstRepeat());
        }

        void FirstRepeat()
        {
            messageCache.GetOrderedMessages().ForEach(InputMessage);
        }

        public override void InputMessage(MessagePayload toInput)
        {
            OnMessageProcessed(toInput);
        }

        public void Repeat()
        {
            repeatStrategy.Repeat(this, messageCache, systemTime);
        }
    }
}