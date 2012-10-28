using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Expiry;
using SystemDot.Messaging.Channels.Filtering;
using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Configuration.RequestReply
{
    public class RequestReplySenderConfiguration : Initialiser
    {
        readonly RequestSendChannelSchema sendChannelSchema;
        readonly ReplyRecieveChannelSchema recieveChannelSchema;

        public RequestReplySenderConfiguration(
            EndpointAddress address, 
            EndpointAddress recieverAddress, 
            List<Action> buildActions)
            : base(buildActions)
        {
            this.sendChannelSchema = new RequestSendChannelSchema
            {
                FilteringStrategy = new PassThroughMessageFilterStategy(),
                FromAddress = address,
                RecieverAddress = recieverAddress,
                ExpiryStrategy = new PassthroughMessageExpiryStrategy() 
            };

            this.recieveChannelSchema = new ReplyRecieveChannelSchema
            {
                Address = address
            };   
        }

        public RequestReplySenderConfiguration WithHook(IMessageProcessor<object, object> hook)
        {
            Contract.Requires(hook != null);

            this.recieveChannelSchema.Hooks.Add(hook);
            return this;
        }

        protected override void Build()
        {
            Resolve<RequestSendChannelBuilder>().Build(this.sendChannelSchema);
            Resolve<ReplyRecieveChannelBuilder>().Build(this.recieveChannelSchema);
            Resolve<IMessageReciever>().StartPolling(GetAddress());
        }

        protected override EndpointAddress GetAddress()
        {
            return this.sendChannelSchema.FromAddress;
        }

        public RequestReplySenderConfiguration OnlyForMessages(IMessageFilterStrategy toFilterMessagesWith)
        {
            Contract.Requires(toFilterMessagesWith != null);

            this.sendChannelSchema.FilteringStrategy = toFilterMessagesWith;
            return this;
        }

        public RequestReplySenderConfiguration WithDurability()
        {
            this.sendChannelSchema.IsDurable = true;
            this.recieveChannelSchema.IsDurable = true;
            return this;
        }

        public RequestReplySenderConfiguration WithMessageExpiry(IMessageExpiryStrategy strategy)
        {
            Contract.Requires(strategy != null);

            this.sendChannelSchema.ExpiryStrategy = strategy;
            return this;
        }
    }
}