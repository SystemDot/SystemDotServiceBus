using System;
using System.Collections.Generic;
using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Processing.Filtering;
using SystemDot.Messaging.Transport;
using SystemDot.Messaging.Transport.Http.LongPolling;

namespace SystemDot.Messaging.Configuration.RequestReply
{
    public class RequestReplySenderConfiguration : Initialiser
    {
        readonly EndpointAddress address;
        readonly EndpointAddress recieverAddress;
        readonly List<IMessageProcessor<object, object>> hooks;
        IMessageFilterStrategy messageFilterStrategy;

        public RequestReplySenderConfiguration(EndpointAddress address, EndpointAddress recieverAddress, List<Action> buildActions)
            : base(buildActions)
        {
            this.messageFilterStrategy = new PassThroughMessageFilterStategy();
            this.address = address;
            this.recieverAddress = recieverAddress;
            this.hooks = new List<IMessageProcessor<object, object>>();
        }

        public RequestReplySenderConfiguration WithHook(IMessageProcessor<object, object> hook)
        {
            this.hooks.Add(hook);
            return this;
        }

        protected override void Build()
        {
            Resolve<IRequestSendChannelBuilder>().Build(this.messageFilterStrategy, this.address, this.recieverAddress);
            Resolve<IReplyRecieveChannelBuilder>().Build(this.address, this.hooks.ToArray());
            
            Resolve<IMessageReciever>().RegisterListeningAddress(this.address);
        }

        protected override EndpointAddress GetAddress()
        {
            return this.address;
        }

        public RequestReplySenderConfiguration OnlyForMessages(IMessageFilterStrategy toFilterMessagesWith)
        {
            this.messageFilterStrategy = toFilterMessagesWith;
            return this;
        }
    }
}