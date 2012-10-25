
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Expiry;
using SystemDot.Messaging.Channels.Filtering;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Channels.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class RequestSendChannelBuilder
    {
        readonly IMessageSender messageSender;
        readonly ISerialiser serialiser;
        readonly ICurrentDateProvider currentDateProvider;
        readonly ITaskRepeater taskRepeater;
        readonly PersistenceFactorySelector persistenceFactory;
        readonly MessageAcknowledgementHandler acknowledgementHandler;

        public RequestSendChannelBuilder(
            IMessageSender messageSender, 
            ISerialiser serialiser, 
            ICurrentDateProvider currentDateProvider, 
            ITaskRepeater taskRepeater, 
            PersistenceFactorySelector persistenceFactory, 
            MessageAcknowledgementHandler acknowledgementHandler)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(currentDateProvider != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(persistenceFactory != null);
            Contract.Requires(acknowledgementHandler != null);
            
            this.messageSender = messageSender;
            this.serialiser = serialiser;
            this.currentDateProvider = currentDateProvider;
            this.taskRepeater = taskRepeater;
            this.persistenceFactory = persistenceFactory;
            this.acknowledgementHandler = acknowledgementHandler;
        }

        public void Build(RequestSendChannelSchema schema)
        {
            IPersistence persistence = this.persistenceFactory.Select(schema)
                .CreatePersistence(
                    PersistenceUseType.RequestSend, 
                    schema.FromAddress);

            IMessageCache cache = new MessageCache(persistence);

            this.acknowledgementHandler.RegisterPersistence(persistence);

            MessagePipelineBuilder.Build()
                .WithBusSendTo(new MessageFilter(schema.FilteringStrategy))
                .ToConverter(new MessagePayloadPackager(this.serialiser))
                .ToProcessor(new Sequencer(persistence))
                .ToProcessor(new MessageAddresser(schema.FromAddress, schema.RecieverAddress))
                .ToMessageRepeater(cache, this.currentDateProvider, this.taskRepeater)
                .ToProcessor(new MessageCacher(cache))
                .Pump()
                .ToProcessor(new MessageExpirer(schema.ExpiryStrategy, persistence))
                .ToEndPoint(this.messageSender);
        }
    }
}