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
        readonly ReplySendChannelSchema sendChannelSchema;

        public RequestReplyRecieverConfiguration(EndpointAddress address, List<Action> buildActions) : base(buildActions)
        {
            this.sendChannelSchema = new ReplySendChannelSchema
            {
                FromAddress = address
            };
        }

        protected override void Build()
        {
            Resolve<RequestRecieveChannelBuilder>().Build(GetAddress());
            Resolve<ReplySendChannelBuilder>().Build(this.sendChannelSchema);
            Resolve<IMessageReciever>().StartPolling(GetAddress());
        }

        protected override EndpointAddress GetAddress()
        {
            return this.sendChannelSchema.FromAddress;
        }

        public RequestReplyRecieverConfiguration WithPersistence()
        {
            this.sendChannelSchema.IsPersistent = true;
            return this;
        }
    }
}