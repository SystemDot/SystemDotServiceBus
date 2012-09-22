using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Messages.Processing.Caching;
using SystemDot.Messaging.Messages.Processing.Repeating;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class SubscriptionRequestChannelBuilder : ISubscriptionRequestChannelBuilder
    {
        readonly IMessageSender messageSender;
        readonly ICurrentDateProvider currentDateProvider;
        readonly ITaskRepeater taskRepeater;
        readonly IAcknowledgementChannelBuilder acknowledgementChannelBuilder;

        public SubscriptionRequestChannelBuilder(
            IMessageSender messageSender, 
            ICurrentDateProvider currentDateProvider, 
            ITaskRepeater taskRepeater, 
            IAcknowledgementChannelBuilder acknowledgementChannelBuilder)
        {
            Contract.Requires(messageSender != null);
            Contract.Requires(currentDateProvider != null);
            Contract.Requires(taskRepeater != null);
            Contract.Requires(acknowledgementChannelBuilder != null);

            this.messageSender = messageSender;
            this.currentDateProvider = currentDateProvider;
            this.taskRepeater = taskRepeater;
            this.acknowledgementChannelBuilder = acknowledgementChannelBuilder;
        }

        public ISubscriptionRequestor Build(EndpointAddress subscriberAddress, EndpointAddress publisherAddress)
        {
            var requestor = new SubscriptionRequestor(subscriberAddress);

            IMessageCache cache = new MessageCache(new InMemoryPersistence(), subscriberAddress);

            MessagePipelineBuilder.Build()
                .With(requestor)
                .ToProcessor(new MessageAddresser(subscriberAddress, publisherAddress))
                .ToMessageRepeater(cache, this.currentDateProvider, this.taskRepeater)
                .ToProcessor(new MessageCacher(cache))
                .ToEndPoint(this.messageSender);

            this.acknowledgementChannelBuilder.Build(cache, subscriberAddress);

            return requestor;
        }
    }
}