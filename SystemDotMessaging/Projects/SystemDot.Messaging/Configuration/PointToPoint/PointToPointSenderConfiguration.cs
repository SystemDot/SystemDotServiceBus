using System;
using System.Collections.Generic;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Expiry;
using SystemDot.Messaging.PointToPoint.Builders;
using SystemDot.Messaging.Repeating;

namespace SystemDot.Messaging.Configuration.PointToPoint
{
    public class PointToPointSenderConfiguration : Initialiser
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

        public PointToPointSenderConfiguration WithDurability()
        {
            this.sendSchema.IsDurable = true;
            return this;
        }
    }
}