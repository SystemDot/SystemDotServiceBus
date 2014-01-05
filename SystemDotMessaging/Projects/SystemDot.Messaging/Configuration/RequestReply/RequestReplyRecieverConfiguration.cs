using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration.ExceptionHandling;
using SystemDot.Messaging.Configuration.Expiry;
using SystemDot.Messaging.Configuration.Filtering;
using SystemDot.Messaging.Configuration.Repeating;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.RequestReply.Builders;
using SystemDot.Messaging.UnitOfWork;

namespace SystemDot.Messaging.Configuration.RequestReply
{
    public class RequestReplyRecieverConfiguration : Configurer, 
        IExceptionHandlingConfigurer,
        IRepeatMessagesConfigurer,
        IExpireMessagesConfigurer,
        IFilterMessagesConfigurer
    {
        readonly ReplySendChannelSchema replySchema;
        readonly RequestRecieveChannelSchema requestSchema;
        readonly ISystemTime systemTime;

        public RequestReplyRecieverConfiguration(EndpointAddress address, MessagingConfiguration messagingConfiguration, ISystemTime systemTime)
            : base(messagingConfiguration)
        {
            this.systemTime = systemTime;

            replySchema = new ReplySendChannelSchema
            {
                FromAddress = address,
                ExpiryStrategy = new PassthroughMessageExpiryStrategy(),
                ExpiryAction = () => { }
            };

            RepeatMessages().WithDefaultEscalationStrategy();

            requestSchema = new RequestRecieveChannelSchema
            {
                Address = address,
                UnitOfWorkRunnerCreator = CreateUnitOfWorkRunner<NullUnitOfWorkFactory>,
                FilterStrategy = new PassThroughMessageFilterStategy()
            };
        }

        protected internal override void Build()
        {
            Resolve<ReplySendDistributionChannelBuilder>().Build(this.replySchema);
            Resolve<RequestReceiveDistributionChannelBuilder>().Build(this.requestSchema);
        }

        protected override MessageServer GetMessageServer()
        {
            return requestSchema.Address.Server;
        }

        public RequestReplyRecieverConfiguration Sequenced()
        {
            requestSchema.IsSequenced = true;
            return this;
        }

        public RequestReplyRecieverConfiguration WithDurability()
        {
            replySchema.IsDurable = true;
            requestSchema.IsDurable = true;
            return this;
        }


        public void SetMessageExpiryStrategy(IMessageExpiryStrategy strategy)
        {
            replySchema.ExpiryStrategy = strategy;
        }

        public ExpireMessagesConfiguration<RequestReplyRecieverConfiguration> ExpireMessages()
        {
            return new ExpireMessagesConfiguration<RequestReplyRecieverConfiguration>(this, systemTime);
        }

        public RequestReplyRecieverConfiguration OnMessageExpiry(Action expiryAction)
        {
            Contract.Requires(expiryAction != null);

            replySchema.ExpiryAction = expiryAction;

            return this;
        }

        public RepeatMessagesConfiguration<RequestReplyRecieverConfiguration> RepeatMessages()
        {
            return new RepeatMessagesConfiguration<RequestReplyRecieverConfiguration>(this);
        }

        public void SetMessageRepeatingStrategy(IRepeatStrategy strategy)
        {
            Contract.Requires(strategy != null);

            replySchema.RepeatStrategy = strategy;
        }

        public RequestReplyRecieverConfiguration WithUnitOfWork<TUnitOfWorkFactory>()
            where TUnitOfWorkFactory : class, IUnitOfWorkFactory
        {
            requestSchema.UnitOfWorkRunnerCreator = CreateUnitOfWorkRunner<TUnitOfWorkFactory>;
            return this;
        }

        public RequestReplyRecieverConfiguration WithReceiveHook(IMessageHook<object> hook)
        {
            Contract.Requires(hook != null);

            requestSchema.Hooks.Add(hook);
            return this;
        }

        public RequestReplyRecieverConfiguration WithReceiveHook(IMessageHook<MessagePayload> hook)
        {
            Contract.Requires(hook != null);

            requestSchema.PreUnpackagingHooks.Add(hook);
            return this;
        }

        public RequestReplyRecieverConfiguration WithReplyHook(IMessageHook<object> hook)
        {
            Contract.Requires(hook != null);

            replySchema.Hooks.Add(hook);
            return this;
        }

        public FilterMessagesConfiguration<RequestReplyRecieverConfiguration> OnlyForMessages()
        {
            return new FilterMessagesConfiguration<RequestReplyRecieverConfiguration>(this);
        }

        public void SetMessageFilterStrategy(IMessageFilterStrategy strategy)
        {
            requestSchema.FilterStrategy = strategy;
        }

        public RequestReplyRecieverConfiguration HandleRequestsOnMainThread()
        {
            requestSchema.HandleRequestsOnMainThread = true;
            return this;
        }

        public OnExceptionConfiguration<RequestReplyRecieverConfiguration> OnException()
        {
            return new OnExceptionConfiguration<RequestReplyRecieverConfiguration>(this);
        }

        public void SetContinueOnException()
        {
            requestSchema.ContinueOnException = true;
        }

        public RequestReplyRecieverConfiguration InBlockMessagesMode()
        {
            requestSchema.BlockMessages = true;
            return this;
        }
    }
}