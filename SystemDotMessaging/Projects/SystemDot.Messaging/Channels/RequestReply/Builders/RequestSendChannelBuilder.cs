using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Acknowledgement.Builders;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Filtering;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Storage;
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
        readonly IAcknowledgementChannelBuilder acknowledgementChannelBuilder;
        readonly ITaskRepeater taskRepeater;

        public RequestSendChannelBuilder(
            IMessageSender messageSender, 
            ISerialiser serialiser, 
            IPersistence persistence, 
            ICurrentDateProvider currentDateProvider, 
            IAcknowledgementChannelBuilder acknowledgementChannelBuilder, 
            ITaskRepeater taskRepeater)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(serialiser != null);
            Contract.Requires(persistence != null);
            Contract.Requires(currentDateProvider != null);
            Contract.Requires(acknowledgementChannelBuilder != null);
            Contract.Requires(taskRepeater != null);

            this.messageSender = messageSender;
            this.serialiser = serialiser;
            this.persistence = persistence;
            this.currentDateProvider = currentDateProvider;
            this.acknowledgementChannelBuilder = acknowledgementChannelBuilder;
            this.taskRepeater = taskRepeater;
        }

        public void Build(IMessageFilterStrategy filteringStrategy, EndpointAddress fromAddress, EndpointAddress recieverAddress)
        {
            IMessageCache cache = new MessageCache(this.persistence, fromAddress); 

            MessagePipelineBuilder.Build()
                .WithBusSendTo(new MessageFilter(filteringStrategy))
                .ToConverter(new MessagePayloadPackager(this.serialiser))
                .ToProcessor(new MessageAddresser(fromAddress, recieverAddress))
                .ToMessageRepeater(cache, this.currentDateProvider, this.taskRepeater)
                .ToProcessor(new MessageCacher(cache))
                .Pump()
                .ToEndPoint(this.messageSender);

            this.acknowledgementChannelBuilder.Build(cache, fromAddress);
        }
    }
}