using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Caching;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;

namespace SystemDot.Messaging.Publishing.Builders
{
    class SubscriptionRequestChannelBuilder
    {
        readonly IMessageSender messageSender;
        readonly ISystemTime systemTime;
        readonly ITaskRepeater taskRepeater;
        readonly InMemoryChangeStore changeStore;
        readonly MessageAcknowledgementHandler acknowledgementHandler;
        readonly ISerialiser serialiser;

        public SubscriptionRequestChannelBuilder(
            IMessageSender messageSender, 
            ISystemTime systemTime, 
            ITaskRepeater taskRepeater,
            InMemoryChangeStore changeStore, 
            MessageAcknowledgementHandler acknowledgementHandler, 
            ISerialiser serialiser)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(changeStore != null);
            Contract.Requires(acknowledgementHandler != null);
            Contract.Requires(serialiser != null);

            this.messageSender = messageSender;
            this.systemTime = systemTime;
            this.taskRepeater = taskRepeater;
            this.changeStore = changeStore;
            this.acknowledgementHandler = acknowledgementHandler;
            this.serialiser = serialiser;
        }

        public ISubscriptionRequestor Build(SubscriptionRequestChannelSchema schema)
        {
            var requestor = new SubscriptionRequestor(schema.SubscriberAddress, schema.IsDurable);
            
            SendMessageCache messageCache = new MessageCacheFactory(this.changeStore, this.systemTime)
                .CreateSendCache(
                    PersistenceUseType.SubscriberRequestSend, 
                    schema.PublisherAddress);

            this.acknowledgementHandler.RegisterCache(messageCache);

            MessagePipelineBuilder.Build()
                .With(requestor)
                .ToProcessor(new MessageAddresser(schema.SubscriberAddress, schema.PublisherAddress))
                .ToMessageRepeater(messageCache, this.systemTime, this.taskRepeater, EscalatingTimeRepeatStrategy.Default)
                .ToProcessor(new SendChannelMessageCacher(messageCache))
                .ToProcessor(new PersistenceSourceRecorder())
                .Pump()
                .ToEndPoint(this.messageSender);

            return requestor;
        }
    }
}