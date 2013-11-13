using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration.ExceptionHandling;
using SystemDot.Messaging.Configuration.Expiry;
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
    public class RequestReplySenderConfiguration : Configurer, 
        IExceptionHandlingConfigurer,
        IRepeatMessagesConfigurer,
        IExpireMessagesConfigurer
    {
        readonly RequestSendChannelSchema sendSchema;
        readonly ReplyReceiveChannelSchema receiveSchema;

        public RequestReplySenderConfiguration(
            EndpointAddress address,
            EndpointAddress recieverAddress,
            MessagingConfiguration messagingConfiguration)
            : base(messagingConfiguration)
        {
            sendSchema = new RequestSendChannelSchema
            {
                FilteringStrategy = new PassThroughMessageFilterStategy(),
                FromAddress = address,
                ReceiverAddress = recieverAddress,
                ExpiryStrategy = new PassthroughMessageExpiryStrategy(),
                ExpiryAction = () => { }
            };

            RepeatMessages().WithDefaultEscalationStrategy();

            receiveSchema = new ReplyReceiveChannelSchema
            {
                Address = address,
                ToAddress = recieverAddress,
                UnitOfWorkRunnerCreator = CreateUnitOfWorkRunner<NullUnitOfWorkFactory>
            };
        }

        protected internal override void Build()
        {
            Resolve<RequestSendChannelBuilder>().Build(sendSchema);
            Resolve<ReplyReceiveChannelBuilder>().Build(receiveSchema);
        }

        protected override MessageServer GetMessageServer()
        {
            return sendSchema.FromAddress.Server;
        }

        public RequestReplySenderConfiguration OnlyForMessages(IMessageFilterStrategy toFilterMessagesWith)
        {
            Contract.Requires(toFilterMessagesWith != null);

            sendSchema.FilteringStrategy = toFilterMessagesWith;
            return this;
        }

        public RequestReplySenderConfiguration Sequenced()
        {
            receiveSchema.IsSequenced = true;
            return this;
        }

        public RequestReplySenderConfiguration WithDurability()
        {
            sendSchema.IsDurable = true;
            receiveSchema.IsDurable = true;
            return this;
        }

        public ExpireMessagesConfiguration<RequestReplySenderConfiguration> ExpireMessages()
        {
            return new ExpireMessagesConfiguration<RequestReplySenderConfiguration>(this, Resolve<ISystemTime>());
        }

        public RequestReplySenderConfiguration OnMessageExpiry(Action expiryAction)
        {
            Contract.Requires(expiryAction != null);

            sendSchema.ExpiryAction = expiryAction;
            return this;
        }

        public void SetMessageExpiryStrategy(IMessageExpiryStrategy strategy)
        {
            sendSchema.ExpiryStrategy = strategy;
        }

        public RepeatMessagesConfiguration<RequestReplySenderConfiguration> RepeatMessages()
        {
            return new RepeatMessagesConfiguration<RequestReplySenderConfiguration>(this);
        }

        public void SetMessageRepeatingStrategy(IRepeatStrategy strategy)
        {
            Contract.Requires(strategy != null);

            sendSchema.RepeatStrategy = strategy;
        }

        public RequestReplySenderConfiguration WithUnitOfWork<TUnitOfWorkFactory>()
            where TUnitOfWorkFactory : class, IUnitOfWorkFactory
        {
            receiveSchema.UnitOfWorkRunnerCreator = CreateUnitOfWorkRunner<TUnitOfWorkFactory>;
            return this;
        }

        public RequestReplySenderConfiguration WithReceiveHook(IMessageHook<object> hook)
        {
            Contract.Requires(hook != null);

            receiveSchema.Hooks.Add(hook);
            return this;
        }

        public RequestReplySenderConfiguration WithSendHook(IMessageHook<object> hook)
        {
            Contract.Requires(hook != null);

            sendSchema.Hooks.Add(hook);
            return this;
        }

        public RequestReplySenderConfiguration WithSendHook(IMessageHook<MessagePayload> hook)
        {
            Contract.Requires(hook != null);

            sendSchema.PostPackagingHooks.Add(hook);
            return this;
        }

        public RequestReplySenderConfiguration HandleRepliesOnMainThread()
        {
            receiveSchema.HandleRepliesOnMainThread = true;
            return this;
        }

        public OnExceptionConfiguration<RequestReplySenderConfiguration> OnException()
        {
            return new OnExceptionConfiguration<RequestReplySenderConfiguration>(this);
        }

        public RequestReplySenderConfiguration CorrelateReplyToRequest()
        {
            receiveSchema.CorrelateReplyToRequest = true;
            sendSchema.CorrelateReplyToRequest = true;
            return this;
        }

        public void SetContinueOnException()
        {
            receiveSchema.ContinueOnException = true;
        }
    }
}