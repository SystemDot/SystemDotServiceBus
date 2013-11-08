using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Authentication;
using SystemDot.Messaging.Authentication.Caching;
using SystemDot.Messaging.Caching;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Publishing.Builders
{
    class SubscriptionRequestSendChannelBuilder
    {
        readonly MessageSender messageSender;
        readonly ISystemTime systemTime;
        readonly ITaskRepeater taskRepeater;
        readonly MessageAcknowledgementHandler acknowledgementHandler;
        readonly AuthenticationSessionCache authenticationSessionCache;

        public SubscriptionRequestSendChannelBuilder(
            MessageSender messageSender, 
            ISystemTime systemTime, 
            ITaskRepeater taskRepeater, 
            MessageAcknowledgementHandler acknowledgementHandler, 
            AuthenticationSessionCache authenticationSessionCache)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(acknowledgementHandler != null);
            Contract.Requires(authenticationSessionCache != null);

            this.messageSender = messageSender;
            this.systemTime = systemTime;
            this.taskRepeater = taskRepeater;
            this.acknowledgementHandler = acknowledgementHandler;
            this.authenticationSessionCache = authenticationSessionCache;
        }

        public void Build(SubscriptionRequestChannelSchema schema)
        {
            SendMessageCache cache = new MessageCacheFactory(new NullChangeStore(), systemTime)
                .CreateSendCache(
                    PersistenceUseType.SubscriberRequestSend, 
                    schema.PublisherAddress);

            acknowledgementHandler.RegisterCache(cache);

            MessagePipelineBuilder.Build()
                .With(new SubscriptionRequestor(schema.SubscriberAddress, schema.IsDurable))
                .ToProcessor(new MessageAddresser(schema.SubscriberAddress, schema.PublisherAddress))
                .ToProcessor(new SendChannelMessageCacher(cache))
                .ToMessageRepeater(cache, systemTime, taskRepeater, EscalatingTimeRepeatStrategy.Default)
                .ToProcessor(new SendChannelMessageCacheUpdater(cache))
                .ToProcessor(new PersistenceSourceRecorder())
                .Pump()
                .ToProcessor(new LastSentRecorder(systemTime))
                .ToProcessor(new AuthenticationSessionAttacher(authenticationSessionCache, schema.PublisherAddress))
                .ToEndPoint(messageSender);
        }
    }
}