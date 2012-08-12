using System;
using System.Collections.Generic;
using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Transport;
using SystemDot.Messaging.Transport.Http.LongPolling;

namespace SystemDot.Messaging.Configuration.RequestReply
{
    public class RequestReplySenderConfiguration : Initialiser
    {
        readonly EndpointAddress address;
        readonly EndpointAddress recieverAddress;
        readonly List<IMessageProcessor<object, object>> hooks;

        public RequestReplySenderConfiguration(EndpointAddress address, EndpointAddress recieverAddress, List<Action> buildActions)
            : base(buildActions)
        {
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
            Resolve<ISendChannelBuilder>().Build(this.recieverAddress);
            Resolve<IRecieveChannelBuilder>().Build(this.hooks.ToArray());
            Resolve<ISubscriptionHandlerChannelBuilder>().Build(this.address, this.recieverAddress).Start();
            Resolve<IMessageReciever>().RegisterListeningAddress(this.address);
        }

        protected override EndpointAddress GetAddress()
        {
            return this.address;
        }
    }
}