using System;
using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Consuming;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Messages.Processing;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Configuration.RequestReply
{
    public class RequestReplySenderConfiguration : Configurer
    {
        readonly EndpointAddress address;
        readonly EndpointAddress recieverAddress;

        public RequestReplySenderConfiguration(EndpointAddress address, EndpointAddress recieverAddress)
        {
            this.address = address;
            this.recieverAddress = recieverAddress;
        }

        public IBus Initialise()
        {
            BuildSendChannel();
            BuildRecieveChannel();
            BuildSubscriptionRequestChannel().Start();

            Resolve<ITaskLooper>().Start();
            return Resolve<IBus>();
        }

        void BuildSendChannel()
        {
            MessagePipelineBuilder.Build()
               .With(Resolve<IBus>())
               .Pump()
               .ToProcessor(Resolve<MessagePayloadPackager>())
               .ToProcessor(Resolve<MessageAddresser, EndpointAddress>(this.recieverAddress))
               .ToEndPoint(Resolve<IMessageSender>());
        }

        void BuildRecieveChannel()
        {
            MessagePipelineBuilder.Build()
               .With(Resolve<IMessageReciever>())
               .Pump()
               .ToProcessor(Resolve<MessagePayloadUnpackager>())
               .ToEndPoint(Resolve<MessageHandlerRouter>());

            Resolve<IMessageReciever>().RegisterListeningAddress(this.address);
        }

        SubscriptionRequestor BuildSubscriptionRequestChannel()
        {
            var requestor = Resolve<SubscriptionRequestor, EndpointAddress>(this.address);

            MessagePipelineBuilder.Build()
                .With(requestor)
                .Pump()
                .ToProcessor(Resolve<MessageAddresser, EndpointAddress>(this.recieverAddress))
                .ToProcessor(Resolve<MessageRepeater, TimeSpan>(new TimeSpan(0, 0, 1)))
                .ToEndPoint(Resolve<IMessageSender>());

            return requestor;
        }
    }
}