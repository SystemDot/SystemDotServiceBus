using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Expiry;
using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Configuration.RequestReply
{
    public class RequestReplyRecieverConfiguration : Initialiser
    {
        readonly ReplySendChannelSchema sendChannelSchema;
        readonly RequestRecieveChannelSchema requestChannelSchema;

        public RequestReplyRecieverConfiguration(EndpointAddress address, List<Action> buildActions) : base(buildActions)
        {
            this.sendChannelSchema = new ReplySendChannelSchema
            {
                FromAddress = address,
                ExpiryStrategy = new PassthroughMessageExpiryStrategy()
            };

            this.requestChannelSchema = new RequestRecieveChannelSchema
            {
                Address = address,
                IsSequenced = false
            };
        }

        protected override void Build()
        {
            Resolve<RequestRecieveChannelBuilder>().Build(this.requestChannelSchema);
            Resolve<ReplySendChannelBuilder>().Build(this.sendChannelSchema);
            Resolve<IMessageReciever>().StartPolling(GetAddress());
        }

        protected override EndpointAddress GetAddress()
        {
            return this.sendChannelSchema.FromAddress;
        }

        public RequestReplyRecieverConfiguration WithDurability()
        {
            this.sendChannelSchema.IsDurable = true;
            this.requestChannelSchema.IsSequenced = true;
            return this;
        }

        public RequestReplyRecieverConfiguration WithMessageExpiry(IMessageExpiryStrategy strategy)
        {
            Contract.Requires(strategy != null);

            this.sendChannelSchema.ExpiryStrategy = strategy;
            return this;
        }
    }
}