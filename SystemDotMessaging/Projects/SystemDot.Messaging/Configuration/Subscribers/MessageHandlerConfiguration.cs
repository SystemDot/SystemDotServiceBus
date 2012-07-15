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
    public class MessageHandlerConfiguration : InitialisingConfiguration
    {
        readonly IMessageHandler toRegister;
        readonly EndpointAddress publisherAddress;

        public MessageHandlerConfiguration(IMessageHandler toRegister, EndpointAddress publisherAddress)
        {
            Contract.Requires(toRegister != null);
            Contract.Requires(publisherAddress != EndpointAddress.Empty);
            
            this.toRegister = toRegister;
            this.publisherAddress = publisherAddress;
        }

        public override IBus Initialise()
        {
            Components.Register();
            BuildSubscriber();
            return IocContainer.Resolve<IBus>();
        }

        void BuildSubscriber()
        {
            var subscriberAddress = new EndpointAddress(
                this.publisherAddress.Channel, 
                Resolve<IMachineIdentifier>().GetMachineName());

            BuildSubscriberChannel(subscriberAddress);
            BuildSubscriptionRequestChannel(subscriberAddress).Start();
            IocContainer.Resolve<TaskLooper>().Start();
        }

        SubscriptionRequestor BuildSubscriptionRequestChannel(EndpointAddress subscriberAddress)
        {
            SubscriptionRequestor requestor = Resolve<SubscriptionRequestor, EndpointAddress>(subscriberAddress);
            
            MessagePipelineBuilder.Build()
                .With(requestor)
                .ToProcessor(Resolve<MessageAddresser, EndpointAddress>(this.publisherAddress))
                .ToProcessor(Resolve<MessageRepeater, TimeSpan>(new TimeSpan(0, 0, 1)))
                .ToEndPoint(Resolve<IMessageSender>());

            return requestor;
        }

        void BuildSubscriberChannel(EndpointAddress subscriberAddress)
        {
            var messageHandlerRouter = Resolve<MessageHandlerRouter>();
            
            MessagePipelineBuilder.Build()
                .With(Resolve<IMessageReciever>())
                .Pump()
                .ToProcessor(Resolve<MessagePayloadUnpackager>())
                .ToEndPoint(messageHandlerRouter);

            Resolve<IMessageReciever>().RegisterListeningAddress(subscriberAddress);
            messageHandlerRouter.RegisterHandler(this.toRegister);
        }
    }
}