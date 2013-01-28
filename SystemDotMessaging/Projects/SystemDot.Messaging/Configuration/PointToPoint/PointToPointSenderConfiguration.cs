using System;
using System.Collections.Generic;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.PointToPoint.Builders;
using SystemDot.Messaging.Channels.Repeating;
using SystemDot.Messaging.Transport;

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
                RepeatStrategy = new EscalatingTimeRepeatStrategy(),
                FromAddress = address
            };
        }

        protected override void Build()
        {
            Resolve<PointToPointSendChannelBuilder>().Build(this.sendSchema);
            Resolve<IMessageReciever>().RegisterAddress(GetAddress());
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