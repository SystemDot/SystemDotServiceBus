using System;
using System.Collections.Generic;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.PointToPoint.Builders;
using SystemDot.Messaging.Repeating;

namespace SystemDot.Messaging.Configuration.PointToPoint
{
    public class PointToPointSenderConfiguration : Initialiser
    {
        readonly PointToPointSendChannelSchema sendSchema;
        
        public PointToPointSenderConfiguration(List<Action> buildActions, EndpointAddress address)
            : base(buildActions)
        {
            this.sendSchema = new PointToPointSendChannelSchema
            {
                RepeatStrategy = EscalatingTimeRepeatStrategy.Default,
                FromAddress = address
            };
        }

        protected override void Build()
        {
            Resolve<PointToPointSendChannelBuilder>().Build(this.sendSchema);
        }

        protected override EndpointAddress GetAddress()
        {
            return this.sendSchema.FromAddress;
        }

        public PointToPointSenderConfiguration WithMessageRepeating(IRepeatStrategy strategy)
        {
            this.sendSchema.RepeatStrategy = strategy;
            return this;
        }
    }
}