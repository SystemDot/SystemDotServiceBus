using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Pipelines;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Configuration.RequestReply
{
    public class RequestReplyRecieverConfiguration : Configurer
    {
        readonly EndpointAddress address;

        public RequestReplyRecieverConfiguration(EndpointAddress address)
        {
            this.address = address;
        }

        public IBus Initialise()
        {
            var reciever = Resolve<IMessageReciever>();

            MessagePipelineBuilder.Build()
                .With(reciever)
                .Pump()
                .ToEndPoint(Resolve<SubscriptionRequestHandler>());

            reciever.RegisterListeningAddress(this.address);

            Resolve<ITaskLooper>().Start();

            return Resolve<IBus>();
        }
    }
}