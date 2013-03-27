using System;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Builders;

namespace SystemDot.Messaging.PointToPoint.Builders
{
    class PointToPointReceiverChannelSchema : ChannelSchema
    {
        public EndpointAddress Address { get; set; }

        public Func<IMessageProcessor<object, object>> UnitOfWorkRunnerCreator { get; set; }
    }
}