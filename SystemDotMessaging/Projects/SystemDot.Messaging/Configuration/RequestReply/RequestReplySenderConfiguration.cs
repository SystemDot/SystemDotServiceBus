using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.Repeating;
using SystemDot.Messaging.RequestReply.Builders;
using SystemDot.Messaging.Transport;
using SystemDot.Messaging.UnitOfWork;

namespace SystemDot.Messaging.Configuration.RequestReply
{
    public class RequestReplySenderConfiguration : Initialiser
    {
        readonly RequestSendChannelSchema sendSchema;
        readonly ReplyRecieveChannelSchema recieveSchema;

        public RequestReplySenderConfiguration(
            EndpointAddress address, 
            EndpointAddress recieverAddress, 
            List<Action> buildActions)
            : base(buildActions)
        {
            this.sendSchema = new RequestSendChannelSchema
            {
                FilteringStrategy = new PassThroughMessageFilterStategy(),
                FromAddress = address,
                RecieverAddress = recieverAddress,
                ExpiryStrategy = new PassthroughMessageExpiryStrategy(),
                RepeatStrategy = EscalatingTimeRepeatStrategy.Default
            };

            this.recieveSchema = new ReplyRecieveChannelSchema
            {
                Address = address,
                UnitOfWorkRunner = CreateUnitOfWorkRunner<NullUnitOfWorkFactory>()
            };
        }

        protected override void Build()
        {
            Resolve<RequestSendChannelBuilder>().Build(this.sendSchema);
            Resolve<ReplyRecieveChannelBuilder>().Build(this.recieveSchema);
            Resolve<IMessageReciever>().RegisterAddress(GetAddress());
        }

        protected override EndpointAddress GetAddress()
        {
            return this.sendSchema.FromAddress;
        }

        public RequestReplySenderConfiguration OnlyForMessages(IMessageFilterStrategy toFilterMessagesWith)
        {
            Contract.Requires(toFilterMessagesWith != null);

            this.sendSchema.FilteringStrategy = toFilterMessagesWith;
            return this;
        }

        public RequestReplySenderConfiguration WithDurability()
        {
            this.sendSchema.IsDurable = true;
            this.recieveSchema.IsDurable = true;
            return this;
        }

        public RequestReplySenderConfiguration WithMessageExpiry(IMessageExpiryStrategy strategy)
        {
            Contract.Requires(strategy != null);

            this.sendSchema.ExpiryStrategy = strategy;
            return this;
        }

        public RequestReplySenderConfiguration WithMessageRepeating(IRepeatStrategy strategy)
        {
            Contract.Requires(strategy != null);

            this.sendSchema.RepeatStrategy = strategy;
            return this;
        }

        public RequestReplySenderConfiguration WithUnitOfWork<TUnitOfWorkFactory>() 
            where TUnitOfWorkFactory : class, IUnitOfWorkFactory
        {
            this.recieveSchema.UnitOfWorkRunner = CreateUnitOfWorkRunner<TUnitOfWorkFactory>();
            return this;
        }

        public RequestReplySenderConfiguration WithReceiveHook(IMessageProcessor<object, object> hook)
        {
            Contract.Requires(hook != null);

            this.recieveSchema.Hooks.Add(hook);
            return this;
        }

        public RequestReplySenderConfiguration WithSendHook(IMessageProcessor<object, object> hook)
        {
            Contract.Requires(hook != null);

            this.sendSchema.Hooks.Add(hook);
            return this;
        }
    }
}