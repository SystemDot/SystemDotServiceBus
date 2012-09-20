using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Messages.Processing.Filtering;
using SystemDot.Messaging.Messages.Processing.Repeating;
using SystemDot.Messaging.Messages.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Channels.RequestReply.Builders
{
    public class RequestSendChannelBuilder : IRequestSendChannelBuilder
    {
        readonly IMessageSender messageSender;
        readonly ISerialiser serialiser;
        readonly IPersistence persistence;
        readonly ICurrentDateProvider currentDateProvider;
        readonly ITaskScheduler taskScheduler;
        readonly IAcknowledgementChannelBuilder acknowledgementChannelBuilder;

        public RequestSendChannelBuilder(
            IMessageSender messageSender, 
            ISerialiser serialiser, 
            IPersistence persistence, 
            ICurrentDateProvider currentDateProvider, 
            ITaskScheduler taskScheduler, 
            IAcknowledgementChannelBuilder acknowledgementChannelBuilder)
        {
            this.messageSender = messageSender;
            this.serialiser = serialiser;
            this.persistence = persistence;
            this.currentDateProvider = currentDateProvider;
            this.taskScheduler = taskScheduler;
            this.acknowledgementChannelBuilder = acknowledgementChannelBuilder;
        }

        public void Build(
            IMessageFilterStrategy filteringStrategy, 
            EndpointAddress fromAddress, 
            EndpointAddress recieverAddress)
        {
            IMessageCache cache = new MessageCache(this.persistence, fromAddress); 

            MessagePipelineBuilder.Build()
                .WithBusSendTo(new MessageFilter(filteringStrategy))
                .ToConverter(new MessagePayloadPackager(this.serialiser))
                .ToProcessor(new MessageAddresser(fromAddress, recieverAddress))
                .ToProcessor(new DurableMessageRepeater(cache, this.currentDateProvider, this.taskScheduler))
                .ToProcessor(new MessageCacher(cache))
                .Pump()
                .ToEndPoint(this.messageSender);

            this.acknowledgementChannelBuilder.Build(cache, fromAddress);
        }
    }
}