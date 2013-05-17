using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.RequestReply.Builders;
using SystemDot.Messaging.Transport;
using SystemDot.Messaging.UnitOfWork;

namespace SystemDot.Messaging.Configuration.RequestReply
{
    public class RequestReplyRecieverConfiguration : Configurer
    {
        readonly ReplySendChannelSchema replySchema;
        readonly RequestRecieveChannelSchema requestSchema;

        public RequestReplyRecieverConfiguration(EndpointAddress address, List<Action> buildActions) 
            : base(buildActions)
        {
            this.replySchema = new ReplySendChannelSchema
            {
                FromAddress = address,
                ExpiryStrategy = new PassthroughMessageExpiryStrategy(),
                ExpiryAction = () => { },
                RepeatStrategy = EscalatingTimeRepeatStrategy.Default
            };

            this.requestSchema = new RequestRecieveChannelSchema
            {
                Address = address,
                UnitOfWorkRunnerCreator = CreateUnitOfWorkRunner<NullUnitOfWorkFactory>
            };
        }

        protected override void Build()
        {
            Resolve<ReplySendDistributionChannelBuilder>().Build(this.replySchema);
            Resolve<RequestReceiveDistributionChannelBuilder>().Build(this.requestSchema);
        }

        protected override ServerPath GetServerPath()
        {
            return this.requestSchema.Address.ServerPath;
        }

        public RequestReplyRecieverConfiguration WithDurability()
        {
            this.replySchema.IsDurable = true;
            this.requestSchema.IsDurable = true;
            return this;
        }

        public RequestReplyRecieverConfiguration WithMessageExpiry(IMessageExpiryStrategy strategy)
        {
            Contract.Requires(strategy != null);

            this.replySchema.ExpiryStrategy = strategy;
            return this;
        }

        public RequestReplyRecieverConfiguration WithMessageExpiry(IMessageExpiryStrategy strategy, Action expiryAction)
        {
            this.replySchema.ExpiryStrategy = strategy;
            this.replySchema.ExpiryAction = expiryAction;

            return this;
        }

        public RequestReplyRecieverConfiguration WithMessageRepeating(IRepeatStrategy strategy)
        {
            Contract.Requires(strategy != null);

            this.replySchema.RepeatStrategy = strategy;
            return this;
        }

        public RequestReplyRecieverConfiguration WithUnitOfWork<TUnitOfWorkFactory>()
            where TUnitOfWorkFactory : class, IUnitOfWorkFactory
        {
            this.requestSchema.UnitOfWorkRunnerCreator = CreateUnitOfWorkRunner<TUnitOfWorkFactory>;
            return this;
        }

        public RequestReplyRecieverConfiguration WithReceiveHook(IMessageProcessor<object, object> hook)
        {
            Contract.Requires(hook != null);

            this.requestSchema.Hooks.Add(hook);
            return this;
        }

        public RequestReplyRecieverConfiguration WithReplyHook(IMessageProcessor<object, object> hook)
        {
            Contract.Requires(hook != null);

            this.replySchema.Hooks.Add(hook);
            return this;
        }
    }
}