using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Publishing.Builders;
using SystemDot.Messaging.UnitOfWork;

namespace SystemDot.Messaging.Configuration.Publishers
{
    public class SubscribeToConfiguration : Configurer
    {
        readonly SubscriptionRequestChannelSchema requestSchema;
        readonly SubscriberRecieveChannelSchema receiveSchema;

        public SubscribeToConfiguration(
            EndpointAddress subscriberAddress, 
            EndpointAddress publisherAddress, 
            MessagingConfiguration messagingConfiguration)
            : base(messagingConfiguration)
        {
            Contract.Requires(subscriberAddress != EndpointAddress.Empty);
            Contract.Requires(publisherAddress != EndpointAddress.Empty);
            Contract.Requires(messagingConfiguration != null);

            this.requestSchema = new SubscriptionRequestChannelSchema
            {
                PublisherAddress = publisherAddress,
                SubscriberAddress = subscriberAddress
            };

            this.receiveSchema = new SubscriberRecieveChannelSchema
            {
                Address = subscriberAddress,
                ToAddress = publisherAddress,
                UnitOfWorkRunnerCreator = CreateUnitOfWorkRunner<NullUnitOfWorkFactory>
            };
        }

        protected override void Build()
        {
            Resolve<SubscriberRecieveChannelBuilder>().Build(this.receiveSchema);
            Resolve<SubscriptionRequestChannelBuilder>().Build(this.requestSchema);
        }

        protected override ServerRoute GetServerPath()
        {
            return this.requestSchema.SubscriberAddress.Route;
        }

        public SubscribeToConfiguration WithDurability()
        {
            this.requestSchema.IsDurable = true;
            this.receiveSchema.IsDurable = true;
            return this;
        }

        public SubscribeToConfiguration WithUnitOfWork<TUnitOfWorkFactory>()
            where TUnitOfWorkFactory : class, IUnitOfWorkFactory
        {
            this.receiveSchema.UnitOfWorkRunnerCreator = CreateUnitOfWorkRunner<TUnitOfWorkFactory>;
            return this;
        }

        public SubscribeToConfiguration WithHook(IMessageProcessor<object, object> hook)
        {
            Contract.Requires(hook != null);

            this.receiveSchema.Hooks.Add(hook);
            return this;
        }
    }
}