using System;
using System.Collections.Generic;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Filtering;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Configuration.Publishers
{
    public class PublisherConfiguration : Initialiser
    {
        readonly EndpointAddress address;
        IMessageFilterStrategy messageFilterStategy = new PassThroughMessageFilterStategy();
            
        public PublisherConfiguration(EndpointAddress address, List<Action> buildActions) : base(buildActions)
        {
            this.address = address;
        }

        protected override void Build()
        {
            Resolve<ISubscriptionHandlerChannelBuilder>().Build();
            Resolve<IPublisherChannelBuilder>().Build(new PublisherChannelSchema(address, this.messageFilterStategy));
            Resolve<IMessageReciever>().StartPolling(address);        
        }

        protected override EndpointAddress GetAddress()
        {
            return this.address;
        }

        public PublisherConfiguration OnlyForMessages(IMessageFilterStrategy toFilterWith)
        {
            this.messageFilterStategy = toFilterWith;
            return this;
        }
    }
}