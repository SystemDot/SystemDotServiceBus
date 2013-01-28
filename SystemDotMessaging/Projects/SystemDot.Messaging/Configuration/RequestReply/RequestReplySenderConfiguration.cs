using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Expiry;
using SystemDot.Messaging.Channels.Filtering;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Channels.RequestReply.Builders;
using SystemDot.Messaging.Channels.UnitOfWork;
using SystemDot.Messaging.Transport;

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
                RepeatStrategy = new EscalatingTimeRepeatStrategy()
            };

            this.recieveSchema = new ReplyRecieveChannelSchema
            {
                Address = address,
                UnitOfWorkRunner = CreateUnitOfWorkRunner<NullUnitOfWorkFactory>()
            };
        }

        public RequestReplySenderConfiguration WithHook(IMessageProcessor<object, object> hook)
        {
            Contract.Requires(hook != null);

            this.recieveSchema.Hooks.Add(hook);
            return this;
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
    }
}