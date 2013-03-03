using System.Diagnostics.Contracts;
using SystemDot.Ioc;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Batching;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Caching;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Parallelism;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.RequestReply.Builders
{
    public class RequestRecieveChannelBuilder
    {
        readonly ReplyAddressLookup replyAddressLookup;
        readonly ISerialiser serialiser;
        readonly MessageHandlerRouter messageHandlerRouter;
        readonly AcknowledgementSender acknowledgementSender;
        readonly PersistenceFactorySelector persistenceFactorySelector;
        readonly ISystemTime systemTime;
        readonly ITaskRepeater taskRepeater;
        
        public RequestRecieveChannelBuilder(
            ReplyAddressLookup replyAddressLookup, 
            ISerialiser serialiser, 
            MessageHandlerRouter messageHandlerRouter, 
            AcknowledgementSender acknowledgementSender,
            PersistenceFactorySelector persistenceFactorySelector, 
            ISystemTime systemTime, 
            ITaskRepeater taskRepeater)
        {
            Contract.Requires(replyAddressLookup != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(messageHandlerRouter != null);
            Contract.Requires(acknowledgementSender != null);
            Contract.Requires(persistenceFactorySelector != null);
            Contract.Requires(systemTime != null);
            Contract.Requires(taskRepeater != null);
            
            this.replyAddressLookup = replyAddressLookup;
            this.serialiser = serialiser;
            this.messageHandlerRouter = messageHandlerRouter;
            this.acknowledgementSender = acknowledgementSender;
            this.persistenceFactorySelector = persistenceFactorySelector;
            this.systemTime = systemTime;
            this.taskRepeater = taskRepeater;
        }

        public IMessageInputter<MessagePayload> Build(RequestRecieveChannelSchema schema, EndpointAddress senderAddress)
        {
            ReceiveMessageCache messageCache = this.persistenceFactorySelector
                .Select(schema)
                .CreateReceiveCache(PersistenceUseType.RequestReceive, senderAddress);

            var startPoint = new MessagePayloadCopier(this.serialiser);

            MessagePipelineBuilder.Build()
                .With(startPoint)
                .ToProcessor(new SequenceOriginApplier(messageCache))
                .ToProcessor(new MessageSendTimeRemover())
                .ToSimpleMessageRepeater(messageCache, this.systemTime, this.taskRepeater)
                .ToProcessor(new MessagePayloadCopier(this.serialiser))
                .ToProcessor(new ReceiveChannelMessageCacher(messageCache))
                .ToProcessor(new MessageAcknowledger(this.acknowledgementSender))
                .Queue()
                .ToResequencerIfSequenced(messageCache, schema)
                .ToProcessor(new ReplyChannelSelector(this.replyAddressLookup))
                .ToConverter(new MessagePayloadUnpackager(this.serialiser))
                .ToProcessor(schema.UnitOfWorkRunner)
                .ToProcessor(new BatchUnpackager())
                .ToProcessors(schema.Hooks.ToArray())
                .ToEndPoint(this.messageHandlerRouter);

            Messenger.Send(new ChannelBuilt
            {
                UseType = PersistenceUseType.RequestReceive,
                Address = senderAddress
            });

            return startPoint;
        }
    }
}