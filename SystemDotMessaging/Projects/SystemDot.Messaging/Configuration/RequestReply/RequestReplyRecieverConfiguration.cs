using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Configuration.RequestReply
{
    public class RequestReplyRecieverConfiguration : Configurer
    {
        private readonly EndpointAddress address;

        public RequestReplyRecieverConfiguration(EndpointAddress address)
        {
            this.address = address;
        }

        public IBus Initialise()
        {
            Resolve<ISubscriptionRequestorChannelBuilder>().Build();
            Resolve<IMessageReciever>().RegisterListeningAddress(this.address);
            Resolve<ITaskLooper>().Start();

            return Resolve<IBus>();
        }
    }
}