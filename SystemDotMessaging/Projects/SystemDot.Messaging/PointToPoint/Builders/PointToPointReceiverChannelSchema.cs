using System;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Filtering;

namespace SystemDot.Messaging.PointToPoint.Builders
{
    class PointToPointReceiverChannelSchema : ChannelSchema
    {
        public EndpointAddress Address { get; set; }

        public Func<IMessageProcessor<object, object>> UnitOfWorkRunnerCreator { get; set; }

        public IMessageFilterStrategy FilterStrategy { get; set; }
    }
}