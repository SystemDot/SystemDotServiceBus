using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Acknowledgement.Builders;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Pipelines;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class SubscriberSendChannelBuilder : ISubscriberSendChannelBuilder
    {
        readonly IMessageSender messageSender;
        readonly IPersistence persistence;
        readonly ICurrentDateProvider currentDateProvider;
        readonly ITaskRepeater taskRepeater;
        readonly IAcknowledgementChannelBuilder acknowledgementChannelBuilder;

        public SubscriberSendChannelBuilder(
            IMessageSender messageSender, 
            IPersistence persistence, 
            ICurrentDateProvider currentDateProvider, 
            ITaskRepeater taskRepeater, 
            IAcknowledgementChannelBuilder acknowledgementChannelBuilder)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(persistence != null);
            Contract.Requires(currentDateProvider != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(acknowledgementChannelBuilder != null);

            this.messageSender = messageSender;
            this.persistence = persistence;
            this.currentDateProvider = currentDateProvider;
            this.taskRepeater = taskRepeater;
            this.acknowledgementChannelBuilder = acknowledgementChannelBuilder;
        }

        public IMessageInputter<MessagePayload> BuildChannel(EndpointAddress fromAddress, EndpointAddress subscriberAddress)
        {
            IMessageCache cache = new MessageCache(this.persistence, subscriberAddress);
            
            var addresser = new MessageAddresser(fromAddress, subscriberAddress);

            MessagePipelineBuilder.Build()
                .With(addresser)
                .ToMessageRepeater(cache, this.currentDateProvider, this.taskRepeater)
                .ToProcessor(new MessageCacher(cache))
                .ToEndPoint(this.messageSender);

            this.acknowledgementChannelBuilder.Build(cache, fromAddress);

            return addresser;
        }
    }
}