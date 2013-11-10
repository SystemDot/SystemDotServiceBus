using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration.Repeating;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.PointToPoint.Builders;
using SystemDot.Messaging.Repeating;

namespace SystemDot.Messaging.Configuration.PointToPoint
{
    public class PointToPointSenderConfiguration : Configurer,
        IRepeatMessagesConfigurer
    {
        readonly PointToPointSendChannelSchema sendSchema;
        
        public PointToPointSenderConfiguration(
            EndpointAddress fromAddress, 
            EndpointAddress toAddress,
            MessagingConfiguration messagingConfiguration)
            : base(messagingConfiguration)
        {
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

        public PointToPointSenderConfiguration WithMessageExpiry(IMessageExpiryStrategy strategy)
        {
            sendSchema.ExpiryStrategy = strategy;
            return this;
        }

        public PointToPointSenderConfiguration WithMessageExpiry(IMessageExpiryStrategy strategy, Action toRunOnExpiry)
        {
            sendSchema.ExpiryStrategy = strategy;
            sendSchema.ExpiryAction = toRunOnExpiry;

            return this;
        }

        public PointToPointSenderConfiguration WithDurability()
        {
            sendSchema.IsDurable = true;
            return this;
        }

        public PointToPointSenderConfiguration OnlyForMessages(IMessageFilterStrategy toFilterMessagesWith)
        {
            Contract.Requires(toFilterMessagesWith != null);

            sendSchema.FilteringStrategy = toFilterMessagesWith;
            return this;
        }
    }
}