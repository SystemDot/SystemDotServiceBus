using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Configuration.ComponentRegistration;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Configuration.RequestReply
{
    public class RequestReplyServerConfiguration : InitialisingConfiguration
    {
        readonly string channel;

        public RequestReplyServerConfiguration(string channel)
        {
            Contract.Requires(!string.IsNullOrEmpty(channel));
            this.channel = channel;
        }

        public override IBus Initialise()
        {
            Components.Register();

            var reciever = Resolve<IMessageReciever>();

            Resolve<IChannelBuilder>().Build(reciever, Resolve<RequestReplySubscriptionHandler>());
            reciever.RegisterListeningAddress(new EndpointAddress(this.channel, Resolve<IMachineIdentifier>().GetMachineName()));

            return Resolve<IBus>();
        }
    }
}