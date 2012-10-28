using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Builders;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Filtering;
using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Channels.RequestReply.Repeating;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Serialisation;
using SystemDot.Messaging.Channels.Sequencing;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class ReplyRecieveChannelBuilder
    {
        readonly ISerialiser serialiser;
        readonly MessageHandlerRouter messageHandlerRouter;
        readonly IMessageReciever messageReciever;
        readonly IMessageSender messageSender;
        readonly PersistenceFactorySelector persistenceFactorySelector;
        readonly ICurrentDateProvider currentDateProvider;

        public ReplyRecieveChannelBuilder(
            ISerialiser serialiser, 
            MessageHandlerRouter messageHandlerRouter, 
            IMessageReciever messageReciever, 
            IMessageSender messageSender,
            PersistenceFactorySelector persistenceFactorySelector, 
            ICurrentDateProvider currentDateProvider)
        {
            Contract.Requires(serialiser != null);
            Contract.Requires(messageHandlerRouter != null);
            Contract.Requires(messageReciever != null);
            Contract.Requires(messageSender != null);
            Contract.Requires(persistenceFactorySelector != null);
            Contract.Requires(currentDateProvider != null);
            
            this.serialiser = serialiser;
            this.messageHandlerRouter = messageHandlerRouter;
            this.messageReciever = messageReciever;
            this.messageSender = messageSender;
            this.persistenceFactorySelector = persistenceFactorySelector;
            this.currentDateProvider = currentDateProvider;
        }

        public void Build(ReplyRecieveChannelSchema schema)
        {
            IPersistence persistence = this.persistenceFactorySelector.Select(schema)
                .CreatePersistence(
                    PersistenceUseType.ReplyReceive,  
                    schema.Address);

            MessagePipelineBuilder.Build()
                .With(this.messageReciever)
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToProcessor(new MessageRepeater(persistence, this.currentDateProvider))
                .ToProcessor(new ReceiveChannelMessageCacher(persistence))
                .Pump()
                .ToProcessor(new MessageAcknowledger(this.messageSender))
                .Queue()
                .ToResequencerIfSequenced(persistence, schema)
                .ToConverter(new MessagePayloadUnpackager(this.serialiser))
                .ToProcessors(schema.Hooks.ToArray())
                .ToEndPoint(this.messageHandlerRouter);
        }
    }
}