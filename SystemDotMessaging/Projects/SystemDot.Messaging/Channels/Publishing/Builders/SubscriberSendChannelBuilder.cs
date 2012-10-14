using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Acknowledgement.Builders;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.InMemory;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class SubscriberSendChannelBuilder
    {
        readonly IMessageSender messageSender;
        readonly IPersistenceFactory persistenceFactory;
        readonly ICurrentDateProvider currentDateProvider;
        readonly ITaskRepeater taskRepeater;
        readonly IAcknowledgementChannelBuilder acknowledgementChannelBuilder;
        
        public SubscriberSendChannelBuilder(
            IMessageSender messageSender, 
            IPersistenceFactory persistenceFactory, 
            ICurrentDateProvider currentDateProvider, 
            ITaskRepeater taskRepeater, 
            IAcknowledgementChannelBuilder acknowledgementChannelBuilder)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(persistenceFactory != null);
            Contract.Requires(currentDateProvider != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(acknowledgementChannelBuilder != null);

            this.messageSender = messageSender;
            this.persistenceFactory = persistenceFactory;
            this.currentDateProvider = currentDateProvider;
            this.taskRepeater = taskRepeater;
            this.acknowledgementChannelBuilder = acknowledgementChannelBuilder;
        }

        public IMessageInputter<MessagePayload> BuildChannel(SubscriberSendChannelSchema schema)
        {
            IMessageCache cache = new MessageCache(GetPersistence(schema));
        
            var addresser = new MessageAddresser(schema.FromAddress, schema.SubscriberAddress);

            MessagePipelineBuilder.Build()
                .With(addresser)
                .ToMessageRepeater(cache, this.currentDateProvider, this.taskRepeater)
                .ToProcessor(new MessageCacher(cache))
                .ToEndPoint(this.messageSender);

            this.acknowledgementChannelBuilder.Build(cache, schema.FromAddress);

            return addresser;
        }

        IPersistence GetPersistence(SubscriberSendChannelSchema schema)
        {
            return schema.IsDurable 
                ? this.persistenceFactory.CreatePersistence(PersistenceUseType.SubscriberSend, schema.SubscriberAddress) 
                : new InMemoryPersistence();
        }
        
    }
}