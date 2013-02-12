using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Ioc;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Publishing.Builders;
using SystemDot.Messaging.Transport;
using SystemDot.Messaging.UnitOfWork;

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
        }

        protected override ServerPath GetServerPath()
        {
            return this.requestSchema.PublisherAddress.ServerPath;
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