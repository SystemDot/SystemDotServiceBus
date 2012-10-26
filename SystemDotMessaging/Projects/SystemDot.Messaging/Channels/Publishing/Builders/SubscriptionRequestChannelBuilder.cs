using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Channels.RequestReply.Repeating;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.InMemory;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class SubscriptionRequestChannelBuilder
    {
        readonly IMessageSender messageSender;
        readonly ICurrentDateProvider currentDateProvider;
        readonly ITaskRepeater taskRepeater;
        readonly InMemoryDatatore inMemoryDatatore;
        readonly MessageAcknowledgementHandler acknowledgementHandler;

        public SubscriptionRequestChannelBuilder(
            IMessageSender messageSender, 
            ICurrentDateProvider currentDateProvider, 
            ITaskRepeater taskRepeater, 
            InMemoryDatatore inMemoryDatatore, 
            MessageAcknowledgementHandler acknowledgementHandler)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(currentDateProvider != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(inMemoryDatatore != null);
            Contract.Requires(acknowledgementHandler != null);

            this.messageSender = messageSender;
            this.currentDateProvider = currentDateProvider;
            this.taskRepeater = taskRepeater;
            this.inMemoryDatatore = inMemoryDatatore;
            this.acknowledgementHandler = acknowledgementHandler;
        }

        public ISubscriptionRequestor Build(SubscriptionRequestChannelSchema schema)
        {
            var requestor = new SubscriptionRequestor(schema.SubscriberAddress, schema.IsPersistent);
            
            IPersistence persistence = new InMemoryPersistenceFactory(this.inMemoryDatatore)
                .CreatePersistence(
                    PersistenceUseType.Other, 
                    schema.PublisherAddress);

            this.acknowledgementHandler.RegisterPersistence(persistence);

            MessagePipelineBuilder.Build()
                .With(requestor)
                .ToProcessor(new MessageAddresser(schema.SubscriberAddress, schema.PublisherAddress))
                .ToMessageRepeater(persistence, this.currentDateProvider, this.taskRepeater)
                .Pump()
                .ToEndPoint(this.messageSender);

            return requestor;
        }
    }
}