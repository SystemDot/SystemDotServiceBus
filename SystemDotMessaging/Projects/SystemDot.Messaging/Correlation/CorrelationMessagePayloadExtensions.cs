using System;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Correlation
{
    public static class CorrelationMessagePayloadExtensions
    {
        public static Guid GetCorrelationId(this MessagePayload payload)
        {
            return payload.GetHeader<CorrelationHeader>().Id;
        }

        public static bool HasCorrelationId(this MessagePayload payload)
        {
            return payload.HasHeader<CorrelationHeader>();
        }
        
        public static void SetCorrelationId(this MessagePayload payload, Guid toSet)
        {
            Contract.Requires(toSet != Guid.Empty);
            payload.AddHeader(new CorrelationHeader { Id = toSet });
        }
    }
}