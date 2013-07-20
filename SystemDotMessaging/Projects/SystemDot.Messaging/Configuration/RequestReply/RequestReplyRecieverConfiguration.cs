using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Hooks;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.RequestReply.Builders;
using SystemDot.Messaging.UnitOfWork;

namespace SystemDot.Messaging.Configuration.RequestReply
{
    public class RequestReplyRecieverConfiguration : Configurer
    {
        readonly ReplySendChannelSchema replySchema;
        readonly RequestRecieveChannelSchema requestSchema;

        public RequestReplyRecieverConfiguration(EndpointAddress address, MessagingConfiguration messagingConfiguration)
            : base(messagingConfiguration)
        {
            replySchema = new ReplySendChannelSchema
            {
                FromAddress = address,
                ExpiryStrategy = new PassthroughMessageExpiryStrategy(),
                ExpiryAction = () => { },
                RepeatStrategy = EscalatingTimeRepeatStrategy.Default
            };

            requestSchema = new RequestRecieveChannelSchema
            {
                Address = address,
                UnitOfWorkRunnerCreator = CreateUnitOfWorkRunner<NullUnitOfWorkFactory>,
                FilterStrategy = new PassThroughMessageFilterStategy()
            };
        }

        protected override void Build()
        {
            Resolve<ReplySendDistributionChannelBuilder>().Build(this.replySchema);
            Resolve<RequestReceiveDistributionChannelBuilder>().Build(this.requestSchema);
        }

        protected override ServerRoute GetServerPath()
        {
            return requestSchema.Address.Route;
        }

        public RequestReplyRecieverConfiguration WithDurability()
        {
            replySchema.IsDurable = true;
            requestSchema.IsDurable = true;

            return this;
        }

        public RequestReplyRecieverConfiguration WithMessageExpiry(IMessageExpiryStrategy strategy)
        {
            Contract.Requires(strategy != null);

            replySchema.ExpiryStrategy = strategy;
            return this;
        }

        public RequestReplyRecieverConfiguration WithMessageExpiry(IMessageExpiryStrategy strategy, Action expiryAction)
        {
            Contract.Requires(strategy != null);
            Contract.Requires(expiryAction != null);

            replySchema.ExpiryStrategy = strategy;
            replySchema.ExpiryAction = expiryAction;

            return this;
        }

        public RequestReplyRecieverConfiguration WithMessageRepeating(IRepeatStrategy strategy)
        {
            Contract.Requires(strategy != null);

            replySchema.RepeatStrategy = strategy;
            return this;
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

        public RequestReplyRecieverConfiguration OnlyForMessages(IMessageFilterStrategy toFilterWith)
        {
            Contract.Requires(toFilterWith != null);

            requestSchema.FilterStrategy = toFilterWith;
            return this;
        }
    }
}