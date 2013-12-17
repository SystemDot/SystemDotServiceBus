using System;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Builders;
using SystemDot.Messaging.Filtering;

namespace SystemDot.Messaging.PointToPoint.Builders
{
    class PointToPointReceiverChannelSchema : ISequenceOptionSchema, IDurableOptionSchema
    {
        public EndpointAddress Address { get; set; }

        public Func<IMessageProcessor<object, object>> UnitOfWorkRunnerCreator { get; set; }

        public IMessageFilterStrategy FilterStrategy { get; set; }

        public bool IsSequenced { get; set; }

        public bool IsDurable { get; set; }

        public bool ContinueOnException { get; set; }

        public bool FlushMessages { get; set; }
    }
}