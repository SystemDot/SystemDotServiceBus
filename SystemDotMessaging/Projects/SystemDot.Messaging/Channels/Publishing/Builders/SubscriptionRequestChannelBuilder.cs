using System;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Channels.Publishing.Builders
{
    public class SubscriptionRequestChannelBuilder : ISubscriptionRequestChannelBuilder
    {
        readonly IMessageSender messageSender;
        readonly ITaskScheduler taskScheduler;

        public SubscriptionRequestChannelBuilder(IMessageSender messageSender, ITaskScheduler taskScheduler)
        {
            this.messageSender = messageSender;
            this.taskScheduler = taskScheduler;
        }

        public ISubscriptionRequestor Build(EndpointAddress subscriberAddress, EndpointAddress publisherAddress)
        {
            var requestor = new SubscriptionRequestor(subscriberAddress);

            MessagePipelineBuilder.Build()
                .With(requestor)
                .ToProcessor(new MessageAddresser(subscriberAddress, publisherAddress))
                .ToProcessor(new MessageRepeater(new TimeSpan(0, 0, 1), this.taskScheduler))
                .ToEndPoint(this.messageSender);

            return requestor;
        }
    }
}