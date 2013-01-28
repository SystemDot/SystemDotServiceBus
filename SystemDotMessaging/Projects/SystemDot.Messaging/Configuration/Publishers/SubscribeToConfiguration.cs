using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Ioc;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Publishing.Builders;
using SystemDot.Messaging.Channels.UnitOfWork;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Configuration.Publishers
{
    public class SubscribeToConfiguration : Initialiser
    {
        readonly SubscriptionRequestChannelSchema requestSchema;
        readonly SubscriberRecieveChannelSchema recieveSchema;

        public SubscribeToConfiguration(
            EndpointAddress subscriberAddress, 
            EndpointAddress publisherAddress, 
            List<Action> buildActions)
            : base(buildActions)
        {
            Contract.Requires(subscriberAddress != EndpointAddress.Empty);
            Contract.Requires(publisherAddress != EndpointAddress.Empty);
            Contract.Requires(buildActions != null);

            this.requestSchema = new SubscriptionRequestChannelSchema
            {
                PublisherAddress = publisherAddress,
                SubscriberAddress = subscriberAddress
            };

            this.recieveSchema = new SubscriberRecieveChannelSchema
            {
                Address = subscriberAddress,
                UnitOfWorkRunner = CreateUnitOfWorkRunner<NullUnitOfWorkFactory>()
            };
        }

        protected override void Build()
        {
            Resolve<SubscriberRecieveChannelBuilder>().Build(this.recieveSchema);
            Resolve<SubscriptionRequestChannelBuilder>().Build(this.requestSchema).Start();
            Resolve<IMessageReciever>().RegisterAddress(this.requestSchema.SubscriberAddress);
        }

        protected override EndpointAddress GetAddress()
        {
            return this.requestSchema.PublisherAddress;
        }

        public SubscribeToConfiguration WithDurability()
        {
            this.requestSchema.IsDurable = true;
            this.recieveSchema.IsDurable = true;
            return this;
        }

        public SubscribeToConfiguration WithUnitOfWork<TUnitOfWorkFactory>()
            where TUnitOfWorkFactory : class, IUnitOfWorkFactory
        {
            this.recieveSchema.UnitOfWorkRunner = CreateUnitOfWorkRunner<TUnitOfWorkFactory>();
            return this;
        }
    }
}