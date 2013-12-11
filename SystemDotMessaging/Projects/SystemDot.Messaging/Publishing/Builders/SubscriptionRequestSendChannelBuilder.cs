using System;
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

namespace SystemDot.Messaging.Publishing.Builders
{
    class SubscriptionRequestSendChannelBuilder
    {
        readonly MessageSender messageSender;
        readonly ISystemTime systemTime;
        readonly ITaskRepeater taskRepeater;
        readonly MessageAcknowledgementHandler acknowledgementHandler;
        readonly AuthenticationSessionCache authenticationSessionCache;
        readonly MessageCacheFactory messageCacheFactory;

        public SubscriptionRequestSendChannelBuilder(
            MessageSender messageSender, 
            ISystemTime systemTime, 
            ITaskRepeater taskRepeater, 
            MessageAcknowledgementHandler acknowledgementHandler, 
            AuthenticationSessionCache authenticationSessionCache, 
            MessageCacheFactory messageCacheFactory)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(acknowledgementHandler != null);
            Contract.Requires(authenticationSessionCache != null);
            Contract.Requires(messageCacheFactory != null);

            this.messageSender = messageSender;
            this.systemTime = systemTime;
            this.taskRepeater = taskRepeater;
            this.acknowledgementHandler = acknowledgementHandler;
            this.authenticationSessionCache = authenticationSessionCache;
            this.messageCacheFactory = messageCacheFactory;
        }

        public void Build(SubscriptionRequestChannelSchema schema)
        {
            SendMessageCache cache = CreateCache(schema);
            RegisterCacheWithAcknowledgementHandler(cache);
            BuildPipeline(schema, cache);
        }

        SendMessageCache CreateCache(SubscriptionRequestChannelSchema schema)
        {
            return messageCacheFactory.BuildNonDurableSendCache(PersistenceUseType.SubscriberRequestSend, schema.PublisherAddress);
        }

        void RegisterCacheWithAcknowledgementHandler(SendMessageCache cache)
        {
            acknowledgementHandler.RegisterCache(cache);
        }

        void BuildPipeline(SubscriptionRequestChannelSchema schema, SendMessageCache cache)
        {
            MessagePipelineBuilder.Build()
                .With(new SubscriptionRequestor(schema))
                .ToProcessor(new MessageAddresser(schema.SubscriberAddress, schema.PublisherAddress))
                .ToProcessor(new SendChannelMessageCacher(cache))
                .ToMessageRepeater(cache, systemTime, taskRepeater, ConstantTimeRepeatStrategy.EveryTenSeconds())
                .ToProcessor(new SendChannelMessageCacheUpdater(cache))
                .ToProcessor(new PersistenceSourceRecorder())
                .Pump()
                .ToProcessor(new LastSentRecorder(systemTime))
                .ToProcessor(new AuthenticationSessionAttacher(authenticationSessionCache, schema.PublisherAddress))
                .ToEndPoint(messageSender);
        }
    }
}