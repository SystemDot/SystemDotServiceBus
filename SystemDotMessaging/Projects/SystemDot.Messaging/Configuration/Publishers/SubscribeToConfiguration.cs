using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration.ExceptionHandling;
using SystemDot.Messaging.Configuration.Filtering;
using SystemDot.Messaging.Configuration.Repeating;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Publishing.Builders;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.UnitOfWork;

namespace SystemDot.Messaging.Configuration.Publishers
{
    public class SubscribeToConfiguration : Configurer, IExceptionHandlingConfigurer, IRepeatMessagesConfigurer, IFilterMessagesConfigurer
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

            requestSchema = new SubscriptionRequestChannelSchema
            {
                PublisherAddress = publisherAddress,
                SubscriberAddress = subscriberAddress
            };

            RepeatMessages().WithDefaultEscalationStrategy();
            
            receiveSchema = new SubscriberRecieveChannelSchema
            {
                Address = subscriberAddress,
                ToAddress = publisherAddress,
                UnitOfWorkRunnerCreator = CreateUnitOfWorkRunner<NullUnitOfWorkFactory>,
                FilterStrategy = new PassThroughMessageFilterStategy()
            };
        }

        protected internal override void Build()
        {
            Resolve<SubscriberRecieveChannelBuilder>().Build(receiveSchema);
            Resolve<SubscriptionRequestSendChannelBuilder>().Build(requestSchema);
        }

        protected override MessageServer GetMessageServer()
        {
            return requestSchema.SubscriberAddress.Server;
        }

        public SubscribeToConfiguration Sequenced()
        {
            receiveSchema.IsSequenced = true;
            return this;
        }

        public SubscribeToConfiguration WithDurability()
        {
            requestSchema.IsDurable = true;
            receiveSchema.IsDurable = true;
            return this;
        }

        public SubscribeToConfiguration WithUnitOfWork<TUnitOfWorkFactory>()
            where TUnitOfWorkFactory : class, IUnitOfWorkFactory
        {
            receiveSchema.UnitOfWorkRunnerCreator = CreateUnitOfWorkRunner<TUnitOfWorkFactory>;
            return this;
        }

        public SubscribeToConfiguration WithHook(IMessageHook<object> hook)
        {
            Contract.Requires(hook != null);

            receiveSchema.Hooks.Add(hook);
            return this;
        }

        public SubscribeToConfiguration WithHook(IMessageHook<MessagePayload> hook)
        {
            Contract.Requires(hook != null);

            receiveSchema.PreUnpackagingHooks.Add(hook);
            return this;
        }

        public FilterMessagesConfiguration<SubscribeToConfiguration> OnlyForMessages()
        {
            return new FilterMessagesConfiguration<SubscribeToConfiguration>(this);
        }

        public void SetMessageFilterStrategy(IMessageFilterStrategy strategy)
        {
            receiveSchema.FilterStrategy = strategy;
        }

        public SubscribeToConfiguration HandleEventsOnMainThread()
        {
            receiveSchema.HandleEventsOnMainThread = true;
            return this;
        }

        public OnExceptionConfiguration<SubscribeToConfiguration> OnException()
        {
            return new OnExceptionConfiguration<SubscribeToConfiguration>(this);
        }

        public void SetContinueOnException()
        {
            receiveSchema.ContinueOnException = true;
        }

        public RepeatMessagesConfiguration<SubscribeToConfiguration> RepeatMessages()
        {
            return new RepeatMessagesConfiguration<SubscribeToConfiguration>(this);
        }

        public void SetMessageRepeatingStrategy(IRepeatStrategy strategy)
        {
            requestSchema.RepeatStrategy = strategy;
        }

        public SubscribeToConfiguration InBlockMessagesMode()
        {
            receiveSchema.BlockMessages = true;
            return this;
        }
    }
}