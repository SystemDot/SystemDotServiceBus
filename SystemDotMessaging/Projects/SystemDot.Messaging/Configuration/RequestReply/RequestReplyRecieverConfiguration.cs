using System;
using System.Collections.Generic;
using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Configuration.RequestReply
{
    public class RequestReplyRecieverConfiguration : Initialiser
    {
        private readonly EndpointAddress address;

        public RequestReplyRecieverConfiguration(EndpointAddress address, List<Action> buildActions) : base(buildActions)
        {
            this.address = address;
        }

        protected override void Build()
        {
            Resolve<ISubscriptionRequestorChannelBuilder>().Build();
            Resolve<IMessageReciever>().RegisterListeningAddress(this.address);
        }

        protected override EndpointAddress GetAddress()
        {
            return this.address;
        }
    }
}