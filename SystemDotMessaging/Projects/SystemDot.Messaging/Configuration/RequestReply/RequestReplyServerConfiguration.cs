using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Configuration.RequestReply
{
    public class RequestReplyServerConfiguration : Configurer
    {
        readonly EndpointAddress address;

        public RequestReplyServerConfiguration(EndpointAddress address)
        {
            this.address = address;
        }

        public IBus Initialise()
        {
            var reciever = Resolve<IMessageReciever>();

            Resolve<IChannelBuilder>().Build(reciever, Resolve<RequestReplySubscriptionHandler>());
            
            reciever.RegisterListeningAddress(this.address);

            return Resolve<IBus>();
        }
    }
}