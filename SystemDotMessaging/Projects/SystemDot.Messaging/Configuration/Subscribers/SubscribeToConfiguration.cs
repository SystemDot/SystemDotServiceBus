using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Consuming;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Configuration.Subscribers
{
    public class SubscribeToConfiguration : Configurer
    {
        readonly EndpointAddress subscriberAddress;
        readonly EndpointAddress publisherAddress;

        public SubscribeToConfiguration(EndpointAddress subscriberAddress, EndpointAddress publisherAddress)
        {
            Contract.Requires(subscriberAddress != EndpointAddress.Empty);
            Contract.Requires(publisherAddress != EndpointAddress.Empty);
            this.subscriberAddress = subscriberAddress;
            this.publisherAddress = publisherAddress;
        }

        public IBus Initialise()
        {
            BuildSubscriber();
            return IocContainer.Resolve<IBus>();
        }

        void BuildSubscriber()
        {
            BuildSubscriberChannel();
            BuildSubscriptionRequestChannel().Start();
            IocContainer.Resolve<ITaskLooper>().Start();
        }

        SubscriptionRequestor BuildSubscriptionRequestChannel()
        {
            SubscriptionRequestor requestor = Resolve<SubscriptionRequestor, EndpointAddress>(
                this.subscriberAddress);

            MessagePipelineBuilder.Build()
                .With(requestor)
                .ToProcessor(Resolve<MessageAddresser, EndpointAddress>(this.publisherAddress))
                .ToProcessor(Resolve<MessageRepeater, TimeSpan>(new TimeSpan(0, 0, 1)))
                .ToEndPoint(Resolve<IMessageSender>());

            return requestor;
        }

        void BuildSubscriberChannel()
        {
            var messageHandlerRouter = Resolve<MessageHandlerRouter>();

            MessagePipelineBuilder.Build()
                .With(Resolve<IMessageReciever>())
                .Pump()
                .ToProcessor(Resolve<MessagePayloadUnpackager>())
                .ToEndPoint(messageHandlerRouter);

            Resolve<IMessageReciever>().RegisterListeningAddress(this.subscriberAddress);
        }
    }
}