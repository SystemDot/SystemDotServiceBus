using System;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Correlation
{
    public class CorrelationHeader : IMessageHeader
    {
        public Guid Id { get; set; }
    }
}