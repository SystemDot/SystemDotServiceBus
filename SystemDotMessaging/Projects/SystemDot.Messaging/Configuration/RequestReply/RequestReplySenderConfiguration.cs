using System;
using System.Collections.Generic;
using SystemDot.Messaging.Channels;
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
                RecieverAddress = recieverAddress
            };

            this.recieveChannelSchema = new ReplyRecieveChannelSchema
            {
                SenderAddress = address
            };   
        }

        public RequestReplySenderConfiguration WithHook(IMessageProcessor<object, object> hook)
        {
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
            this.sendChannelSchema.FilteringStrategy = toFilterMessagesWith;
            return this;
        }

        public RequestReplySenderConfiguration WithPersistence()
        {
            this.sendChannelSchema.IsPersistent = true;
            return this;
        }
    }
}