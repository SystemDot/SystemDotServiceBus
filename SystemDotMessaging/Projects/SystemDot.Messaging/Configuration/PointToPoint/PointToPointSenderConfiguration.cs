using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.Filtering;
using SystemDot.Messaging.PointToPoint.Builders;
using SystemDot.Messaging.Repeating;

namespace SystemDot.Messaging.Configuration.PointToPoint
{
    public class PointToPointSenderConfiguration : Configurer
    {
        readonly PointToPointSendChannelSchema sendSchema;
        
        public PointToPointSenderConfiguration(
            EndpointAddress fromAddress, 
            EndpointAddress toAddress, 
            List<Action> buildActions)
            : base(buildActions)
        {
            this.sendSchema = new PointToPointSendChannelSchema
            {
                RepeatStrategy = EscalatingTimeRepeatStrategy.Default,
                ExpiryStrategy = new PassthroughMessageExpiryStrategy(),
                ExpiryAction = () => { },
                FilteringStrategy = new PassThroughMessageFilterStategy(),
                ReceiverAddress = toAddress,
                FromAddress = fromAddress
            };
        }

        protected override void Build()
        {
            Resolve<PointToPointSendChannelBuilder>().Build(this.sendSchema);
        }

        protected override ServerPath GetServerPath()
        {
            return this.sendSchema.FromAddress.ServerPath;
        }

        public PointToPointSenderConfiguration WithMessageRepeating(IRepeatStrategy strategy)
        {
            this.sendSchema.RepeatStrategy = strategy;
            return this;
        }

        public PointToPointSenderConfiguration WithMessageExpiry(IMessageExpiryStrategy strategy)
        {
            this.sendSchema.ExpiryStrategy = strategy;
            return this;
        }

        public PointToPointSenderConfiguration WithMessageExpiry(IMessageExpiryStrategy strategy, Action toRunOnExpiry)
        {
            this.sendSchema.ExpiryStrategy = strategy;
            this.sendSchema.ExpiryAction = toRunOnExpiry;

            return this;
        }

        public PointToPointSenderConfiguration WithDurability()
        {
            this.sendSchema.IsDurable = true;
            return this;
        }

        public PointToPointSenderConfiguration OnlyForMessages(IMessageFilterStrategy toFilterMessagesWith)
        {
            Contract.Requires(toFilterMessagesWith != null);

            this.sendSchema.FilteringStrategy = toFilterMessagesWith;
            return this;
        }
    }
}