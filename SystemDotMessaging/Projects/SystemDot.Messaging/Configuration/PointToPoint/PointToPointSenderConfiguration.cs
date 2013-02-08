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
        
        public PointToPointSenderConfiguration(EndpointAddress fromAddress, EndpointAddress toAddress, List<Action> buildActions)
            : base(buildActions)
        {
            this.sendSchema = new PointToPointSendChannelSchema
            {
                RepeatStrategy = EscalatingTimeRepeatStrategy.Default,
                RecieverAddress = toAddress,
                FromAddress = fromAddress
            };
        }

        protected override void Build()
        {
            Resolve<PointToPointSendChannelBuilder>().Build(this.sendSchema);
        }

        protected override EndpointAddress GetAddress()
        {
            return this.sendSchema.RecieverAddress;
        }

        public PointToPointSenderConfiguration WithMessageRepeating(IRepeatStrategy strategy)
        {
            this.sendSchema.RepeatStrategy = strategy;
            return this;
        }
    }
}