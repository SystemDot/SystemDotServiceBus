using System;
using System.Diagnostics.Contracts;
using System.Threading;

namespace SystemDot.Messaging.Correlation
{
    class ReplyCorrelationLookup
    {
        readonly ThreadLocal<Guid> currentCorrelationId;

        public ReplyCorrelationLookup()
        {
            this.currentCorrelationId = new ThreadLocal<Guid>();
        }

        public void SetCurrentCorrelationId(Guid toSet)
        {
            Contract.Requires(toSet != Guid.Empty);

            this.currentCorrelationId.Value = toSet;
        }

        public Guid GetCurrentCorrelationId()
        {
            return this.currentCorrelationId.Value;
        }

        public bool HasCurrentCorrelationId()
        {
            return this.currentCorrelationId.IsValueCreated;
        }
    }
}