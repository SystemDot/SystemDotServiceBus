using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Expiry;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Channels.UnitOfWork;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Configuration.RequestReply
{
    public class RequestReplyRecieverConfiguration : Initialiser
    {
        readonly ReplySendChannelSchema sendSchema;
        readonly RequestRecieveChannelSchema requestSchema;

        public RequestReplyRecieverConfiguration(EndpointAddress address, List<Action> buildActions) 
            : base(buildActions)
        {
            this.sendSchema = new ReplySendChannelSchema
            {
                FromAddress = address,
                ExpiryStrategy = new PassthroughMessageExpiryStrategy(),
                RepeatStrategy = EscalatingTimeRepeatStrategy.Default
            };

            this.requestSchema = new RequestRecieveChannelSchema
            {
                Address = address,
                UnitOfWorkRunner = CreateUnitOfWorkRunner<NullUnitOfWorkFactory>()
            };
        }

        protected override void Build()
        {
            Resolve<RequestReceiveDistributionChannelBuilder>().Build(this.requestSchema);
            Resolve<ReplySendDistributionChannelBuilder>().Build(this.sendSchema);
            Resolve<IMessageReciever>().RegisterAddress(GetAddress());
        }

        protected override EndpointAddress GetAddress()
        {
            return this.sendSchema.FromAddress;
        }

        public RequestReplyRecieverConfiguration WithDurability()
        {
            this.sendSchema.IsDurable = true;
            this.requestSchema.IsDurable = true;
            return this;
        }

        public RequestReplyRecieverConfiguration WithMessageExpiry(IMessageExpiryStrategy strategy)
        {
            Contract.Requires(strategy != null);

            this.sendSchema.ExpiryStrategy = strategy;
            return this;
        }

        public RequestReplyRecieverConfiguration WithMessageRepeating(IRepeatStrategy strategy)
        {
            Contract.Requires(strategy != null);

            this.sendSchema.RepeatStrategy = strategy;
            return this;
        }

        public RequestReplyRecieverConfiguration WithUnitOfWork<TUnitOfWorkFactory>()
            where TUnitOfWorkFactory : class, IUnitOfWorkFactory
        {
            this.requestSchema.UnitOfWorkRunner = CreateUnitOfWorkRunner<TUnitOfWorkFactory>();
            return this;
        }
    }
}