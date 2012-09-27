using System;
using System.Collections.Generic;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.RequestReply;
using SystemDot.Messaging.Channels.RequestReply.Builders;
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
            Resolve<RequestRecieveChannelBuilder>().Build(this.address);
            Resolve<ReplySendChannelBuilder>().Build(this.address);
            Resolve<IMessageReciever>().StartPolling(this.address);
        }

        protected override EndpointAddress GetAddress()
        {
            return this.address;
        }
    }
}