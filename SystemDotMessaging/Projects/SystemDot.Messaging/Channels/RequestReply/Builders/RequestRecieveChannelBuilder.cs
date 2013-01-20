using System.Diagnostics.Contracts;
using SystemDot.Ioc;
using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Builders;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Errors;
using SystemDot.Messaging.Channels.Expiry;
using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Channels.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Parallelism;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class RequestRecieveChannelBuilder
    {
        readonly ReplyAddressLookup replyAddressLookup;
        readonly ISerialiser serialiser;
        readonly MessageHandlerRouter messageHandlerRouter;
        readonly AcknowledgementSender acknowledgementSender;
        readonly PersistenceFactorySelector persistenceFactorySelector;
        readonly ICurrentDateProvider currentDateProvider;
        readonly ITaskRepeater taskRepeater;
        readonly IIocContainer iocContainer;
        readonly ErrorReciever errorReciever;

        public RequestRecieveChannelBuilder(
            ReplyAddressLookup replyAddressLookup, 
            ISerialiser serialiser, 
            MessageHandlerRouter messageHandlerRouter, 
            AcknowledgementSender acknowledgementSender,
            PersistenceFactorySelector persistenceFactorySelector, 
            ICurrentDateProvider currentDateProvider, 
            ITaskRepeater taskRepeater, 
            IIocContainer iocContainer, 
            ErrorReciever errorReciever)
        {
            Contract.Requires(replyAddressLookup != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(messageHandlerRouter != null);
            Contract.Requires(acknowledgementSender != null);
            Contract.Requires(persistenceFactorySelector != null);
            Contract.Requires(currentDateProvider != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(iocContainer != null);

            this.replyAddressLookup = replyAddressLookup;
            this.serialiser = serialiser;
            this.messageHandlerRouter = messageHandlerRouter;
            this.acknowledgementSender = acknowledgementSender;
            this.persistenceFactorySelector = persistenceFactorySelector;
            this.currentDateProvider = currentDateProvider;
            this.taskRepeater = taskRepeater;
            this.iocContainer = iocContainer;
            this.errorReciever = errorReciever;
        }

        public IMessageInputter<MessagePayload> Build(RequestRecieveChannelSchema schema, EndpointAddress senderAddress)
        {
            MessageCache messageCache = this.persistenceFactorySelector
                .Select(schema)
                .CreateCache(PersistenceUseType.RequestReceive, senderAddress);

            var startPoint = new MessagePayloadCopier(this.serialiser);

            MessagePipelineBuilder.Build()
                .With(startPoint)
                .ToProcessor(new MessageSendTimeRemover())
                .ToSimpleMessageRepeater(messageCache, this.currentDateProvider, this.taskRepeater)
                .ToProcessor(new MessagePayloadCopier(this.serialiser))
                .ToProcessor(new ReceiveChannelMessageCacher(messageCache))
                .ToProcessor(new MessageAcknowledger(this.acknowledgementSender))
                .Queue()
                .ToResequencerIfSequenced(messageCache, schema)
                .ToProcessor(new ReplyChannelSelector(this.replyAddressLookup))
                .ToProcessorIf(schema.QueueErrors, new ErrorHandler(this.errorReciever))
                .ToConverter(new MessagePayloadUnpackager(this.serialiser))
                .ToProcessor(schema.UnitOfWorkRunner)
                .ToEndPoint(this.messageHandlerRouter);

            return startPoint;
        }
    }
}