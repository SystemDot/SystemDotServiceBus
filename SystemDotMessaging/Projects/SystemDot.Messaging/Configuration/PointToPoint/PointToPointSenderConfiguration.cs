using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration.Expiry;
using SystemDot.Messaging.Configuration.Filtering;
using SystemDot.Messaging.Configuration.Repeating;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.PointToPoint.Builders;
using SystemDot.Messaging.Repeating;

namespace SystemDot.Messaging.Configuration.PointToPoint
{
    public class PointToPointSenderConfiguration : 
        Configurer,
        IRepeatMessagesConfigurer,
        IExpireMessagesConfigurer,
        IFilterMessagesConfigurer
    {
        readonly PointToPointSendChannelSchema sendSchema;
        readonly ISystemTime systemTime;

        public PointToPointSenderConfiguration(
            EndpointAddress fromAddress, 
            EndpointAddress toAddress,
            MessagingConfiguration messagingConfiguration,
            ISystemTime systemTime)
            : base(messagingConfiguration)
        {
            this.systemTime = systemTime;
            sendSchema = new PointToPointSendChannelSchema
            {
                ExpiryStrategy = new PassthroughMessageExpiryStrategy(),
                ExpiryAction = () => { },
                FilteringStrategy = new PassThroughMessageFilterStategy(),
                ReceiverAddress = toAddress,
                FromAddress = fromAddress
            };

            RepeatMessages().WithDefaultEscalationStrategy();
        }

        protected internal override void Build()
        {
            Resolve<PointToPointSendChannelBuilder>().Build(sendSchema);
        }

        protected override MessageServer GetMessageServer()
        {
            return sendSchema.FromAddress.Server;
        }

        public RepeatMessagesConfiguration<PointToPointSenderConfiguration> RepeatMessages()
        {
            return new RepeatMessagesConfiguration<PointToPointSenderConfiguration>(this);
        }

        public void SetMessageRepeatingStrategy(IRepeatStrategy strategy)
        {
            Contract.Requires(strategy != null);

            sendSchema.RepeatStrategy = strategy;
        }

        public ExpireMessagesConfiguration<PointToPointSenderConfiguration> ExpireMessages()
        {
            return new ExpireMessagesConfiguration<PointToPointSenderConfiguration>(this, systemTime);
        }

        public PointToPointSenderConfiguration OnMessagingExpiry(Action toRunOnExpiry)
        {
            sendSchema.ExpiryAction = toRunOnExpiry;
            return this;
        }

        public PointToPointSenderConfiguration WithDurability()
        {
            sendSchema.IsDurable = true;
            return this;
        }

        public FilterMessagesConfiguration<PointToPointSenderConfiguration> OnlyForMessages()
        {
            return new FilterMessagesConfiguration<PointToPointSenderConfiguration>(this);
        }

        public void SetMessageFilterStrategy(IMessageFilterStrategy strategy)
        {
            sendSchema.FilteringStrategy = strategy;
        }

        public void SetMessageExpiryStrategy(IMessageExpiryStrategy strategy)
        {
            sendSchema.ExpiryStrategy = strategy;
        }
    }
}