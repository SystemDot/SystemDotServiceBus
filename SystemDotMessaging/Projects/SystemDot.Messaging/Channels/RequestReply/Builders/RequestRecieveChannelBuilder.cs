using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Builders;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Filtering;
using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Channels.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class RequestRecieveChannelBuilder
    {
        readonly ReplyAddressLookup replyAddressLookup;
        readonly ISerialiser serialiser;
        readonly MessageHandlerRouter messageHandlerRouter;
        readonly IMessageReciever messageReciever;
        readonly IMessageSender messageSender;
        readonly PersistenceFactorySelector persistenceFactorySelector;
        readonly ICurrentDateProvider currentDateProvider;
        readonly ITaskRepeater taskRepeater;

        public RequestRecieveChannelBuilder(
            ReplyAddressLookup replyAddressLookup, 
            ISerialiser serialiser, 
            MessageHandlerRouter messageHandlerRouter, 
            IMessageReciever messageReciever, 
            IMessageSender messageSender,
            PersistenceFactorySelector persistenceFactorySelector, 
            ICurrentDateProvider currentDateProvider, 
            ITaskRepeater taskRepeater)
        {
            Contract.Requires(replyAddressLookup != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(messageHandlerRouter != null);
            Contract.Requires(messageReciever != null);
            Contract.Requires(messageSender != null);
            Contract.Requires(persistenceFactorySelector != null);
            Contract.Requires(currentDateProvider != null);
            Contract.Requires(taskRepeater != null);

            this.replyAddressLookup = replyAddressLookup;
            this.serialiser = serialiser;
            this.messageHandlerRouter = messageHandlerRouter;
            this.messageReciever = messageReciever;
            this.messageSender = messageSender;
            this.persistenceFactorySelector = persistenceFactorySelector;
            this.currentDateProvider = currentDateProvider;
            this.taskRepeater = taskRepeater;
        }

        public void Build(RequestRecieveChannelSchema schema)
        {
            IPersistence persistence = this.persistenceFactorySelector
                .Select(schema)
                .CreatePersistence(PersistenceUseType.RequestReceive, schema.Address);

            MessagePipelineBuilder.Build()
                .With(this.messageReciever)
                .ToProcessor(new MessagePayloadCopier(this.serialiser))
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToEscalatingTimeMessageRepeater(persistence, this.currentDateProvider, this.taskRepeater)
                .ToProcessor(new ReceiveChannelMessageCacher(persistence))
                .Pump()
                .ToProcessor(new MessageAcknowledger(this.messageSender))
                .Queue()
                .ToResequencerIfSequenced(persistence, schema)
                .ToProcessor(new ReplyChannelSelector(this.replyAddressLookup))
                .ToConverter(new MessagePayloadUnpackager(this.serialiser))
                .ToEndPoint(this.messageHandlerRouter);
        }
    }
}