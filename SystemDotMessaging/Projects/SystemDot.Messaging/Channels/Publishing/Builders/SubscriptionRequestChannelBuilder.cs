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
        readonly IDatastore datastore;
        readonly MessageAcknowledgementHandler acknowledgementHandler;

        public SubscriptionRequestChannelBuilder(
            IMessageSender messageSender, 
            ICurrentDateProvider currentDateProvider, 
            ITaskRepeater taskRepeater, 
            IDatastore datastore, 
            MessageAcknowledgementHandler acknowledgementHandler)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(currentDateProvider != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(datastore != null);
            Contract.Requires(acknowledgementHandler != null);

            this.messageSender = messageSender;
            this.currentDateProvider = currentDateProvider;
            this.taskRepeater = taskRepeater;
            this.datastore = datastore;
            this.acknowledgementHandler = acknowledgementHandler;
        }

        public ISubscriptionRequestor Build(SubscriptionRequestChannelSchema schema)
        {
            var requestor = new SubscriptionRequestor(schema.SubscriberAddress, schema.IsDurable);
            
            IPersistence persistence = new InMemoryPersistenceFactory(this.datastore)
                .CreatePersistence(
                    PersistenceUseType.SubscriberRequestSend, 
                    schema.PublisherAddress);

            this.acknowledgementHandler.RegisterPersistence(persistence);

            MessagePipelineBuilder.Build()
                .With(requestor)
                .ToProcessor(new MessageAddresser(schema.SubscriberAddress, schema.PublisherAddress))
                .ToMessageRepeater(persistence, this.currentDateProvider, this.taskRepeater)
                .ToProcessor(new SendChannelMessageCacher(persistence))
                .Pump()
                .ToEndPoint(this.messageSender);

            return requestor;
        }
    }
}