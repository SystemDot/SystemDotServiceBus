using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class SubscriptionRequestChannelBuilder
    {
        readonly IMessageSender messageSender;
        readonly ICurrentDateProvider currentDateProvider;
        readonly ITaskRepeater taskRepeater;
        readonly InMemoryChangeStore changeStore;
        readonly MessageAcknowledgementHandler acknowledgementHandler;

        public SubscriptionRequestChannelBuilder(
            IMessageSender messageSender, 
            ICurrentDateProvider currentDateProvider, 
            ITaskRepeater taskRepeater,
            InMemoryChangeStore changeStore, 
            MessageAcknowledgementHandler acknowledgementHandler)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(currentDateProvider != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(changeStore != null);
            Contract.Requires(acknowledgementHandler != null);

            this.messageSender = messageSender;
            this.currentDateProvider = currentDateProvider;
            this.taskRepeater = taskRepeater;
            this.changeStore = changeStore;
            this.acknowledgementHandler = acknowledgementHandler;
        }

        public ISubscriptionRequestor Build(SubscriptionRequestChannelSchema schema)
        {
            var requestor = new SubscriptionRequestor(schema.SubscriberAddress, schema.IsDurable);
            
            IPersistence persistence = new PersistenceFactory(this.changeStore)
                .CreatePersistence(
                    PersistenceUseType.SubscriberRequestSend, 
                    schema.PublisherAddress);

            this.acknowledgementHandler.RegisterPersistence(persistence);

            MessagePipelineBuilder.Build()
                .With(requestor)
                .ToProcessor(new MessageAddresser(schema.SubscriberAddress, schema.PublisherAddress))
                .ToEscalatingTimeMessageRepeater(persistence, this.currentDateProvider, this.taskRepeater)
                .ToProcessor(new SendChannelMessageCacher(persistence))
                .Pump()
                .ToEndPoint(this.messageSender);

            return requestor;
        }
    }
}