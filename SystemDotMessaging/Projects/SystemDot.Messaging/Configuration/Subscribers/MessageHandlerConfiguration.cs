using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Publishing;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Consuming;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Transport;
using SystemDot.Messaging.Transport.Http.LongPolling;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Configuration.Subscribers
{
    public class MessageHandlerConfiguration : InitialisingConfiguration
    {
        readonly IMessageHandler toRegister;
        readonly EndpointAddress subscriberAddress;
        readonly EndpointAddress publisherAddress;

        public MessageHandlerConfiguration(
            IMessageHandler toRegister, 
            EndpointAddress subscriberAddress,
            EndpointAddress publisherAddress)
        {
            Contract.Requires(toRegister != null);
            Contract.Requires(subscriberAddress != EndpointAddress.Empty);
            Contract.Requires(publisherAddress != EndpointAddress.Empty);
            
            this.toRegister = toRegister;
            this.subscriberAddress = subscriberAddress;
            this.publisherAddress = publisherAddress;
        }

        public override void Initialise()
        {
            Components.Register();
            BuildSubscriber();
        }

        void BuildSubscriber()
        {
            SubscriptionRequestor subscriptionRequestor = BuildSubscriptionRequestChannel();
            BuildSubscriberChannel();

            IocContainer.Resolve<TaskLooper>().Start();
            subscriptionRequestor.Start();
        }

        SubscriptionRequestor BuildSubscriptionRequestChannel()
        {
            SubscriptionRequestor requestor = GetComponent<SubscriptionRequestor, EndpointAddress>(this.subscriberAddress);
            
            MessagePipelineBuilder.Build()
                .With(requestor)
                .ToProcessor(GetComponent<MessageAddresser, EndpointAddress>(this.publisherAddress))
                .ToProcessor(GetComponent<MessageRepeater, TimeSpan>(new TimeSpan(0, 0, 1)))
                .ToEndPoint(GetComponent<IMessageSender>());

            return requestor;
        }

        void BuildSubscriberChannel()
        {
            var messageHandlerRouter = GetComponent<MessageHandlerRouter>();
            
            MessagePipelineBuilder.Build()
                .With(GetComponent<IMessageReciever>())
                .Pump()
                .ToProcessor(GetComponent<MessagePayloadUnpackager>())
                .ToEndPoint(messageHandlerRouter);

            GetComponent<IMessageReciever>().RegisterListeningAddress(this.subscriberAddress);
            messageHandlerRouter.RegisterHandler(this.toRegister);
        }
    }
}