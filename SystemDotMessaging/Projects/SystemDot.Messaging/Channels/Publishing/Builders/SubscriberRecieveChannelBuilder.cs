using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Builders;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Filtering;
using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using SystemDot.Messaging.Channels.Sequencing;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class SubscriberRecieveChannelBuilder
    {
        readonly ISerialiser serialiser;
        readonly MessageHandlerRouter messageHandlerRouter;
        readonly IMessageReciever messageReciever;
        readonly IMessageSender messageSender;
        readonly PersistenceFactorySelector persistenceFactory;
        readonly ICurrentDateProvider currentDateProvider;
        readonly ITaskRepeater taskRepeater;

        public SubscriberRecieveChannelBuilder(
            ISerialiser serialiser, 
            MessageHandlerRouter messageHandlerRouter, 
            IMessageReciever messageReciever, 
            IMessageSender messageSender,
            PersistenceFactorySelector persistenceFactory, 
            ICurrentDateProvider currentDateProvider, 
            ITaskRepeater taskRepeater)
        {
            Contract.Requires(serialiser != null);
            Contract.Requires(messageHandlerRouter != null);
            Contract.Requires(messageReciever != null);
            Contract.Requires(messageSender != null);
            Contract.Requires(persistenceFactory != null);
            Contract.Requires(currentDateProvider != null);
            Contract.Requires(taskRepeater != null);
            
            this.serialiser = serialiser;
            this.messageHandlerRouter = messageHandlerRouter;
            this.messageReciever = messageReciever;
            this.messageSender = messageSender;
            this.persistenceFactory = persistenceFactory;
            this.currentDateProvider = currentDateProvider;
            this.taskRepeater = taskRepeater;
        }

        public void Build(SubscriberRecieveChannelSchema schema)
        {
            IPersistence persistence = this.persistenceFactory
                .Select(schema)
                .CreatePersistence(PersistenceUseType.SubscriberReceive, schema.Address);

            MessagePipelineBuilder.Build()
                .With(this.messageReciever)
                .ToProcessor(new MessagePayloadCopier(this.serialiser))
                .Pump()
                .ToProcessor(new BodyMessageFilter(schema.Address))
                .ToProcessor(new X())
                .ToEscalatingTimeMessageRepeater(persistence, this.currentDateProvider, this.taskRepeater)
                .ToProcessor(new ReceiveChannelMessageCacher(persistence))
                .ToProcessor(new MessageAcknowledger(this.messageSender))
                .Queue()
                .ToResequencerIfSequenced(persistence, schema)
                .ToConverter(new MessagePayloadUnpackager(this.serialiser))
                .ToEndPoint(this.messageHandlerRouter);
        }
    }

    public class X : IMessageProcessor<MessagePayload, MessagePayload>
    {
        public void InputMessage(MessagePayload toInput)
        {
            toInput.RemoveHeader(typeof(LastSentHeader));
            MessageProcessed(toInput);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}