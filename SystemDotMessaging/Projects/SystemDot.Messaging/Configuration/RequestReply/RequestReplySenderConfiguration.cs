using System.Collections.Generic;
using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Transport;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Configuration.RequestReply
{
    public class RequestReplySenderConfiguration : Configurer
    {
        readonly EndpointAddress address;
        readonly EndpointAddress recieverAddress;
        readonly List<IMessageProcessor<object, object>> hooks;

        public RequestReplySenderConfiguration(EndpointAddress address, EndpointAddress recieverAddress)
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

        public IBus Initialise()
        {
            Resolve<ISendChannelBuilder>().Build(this.recieverAddress);
            Resolve<IRecieveChannelBuilder>().Build(this.hooks.ToArray());
            Resolve<ISubscriptionHandlerChannelBuilder>().Build(this.address, this.recieverAddress).Start();

            Resolve<IMessageReciever>().RegisterListeningAddress(this.address);
            Resolve<ITaskLooper>().Start();
            
            return Resolve<IBus>();
        }
    }
}