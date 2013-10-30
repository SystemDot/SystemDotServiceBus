using System;
using System.Collections.Concurrent;
using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Correlation
{
    class CorrelationLookup
    {
        readonly ConcurrentDictionary<Guid, Guid> correlations;

        public CorrelationLookup()
        {
            this.correlations = new ConcurrentDictionary<Guid, Guid>();
        }

        public void RegisterCorrelationStarted(Guid toRegister)
        {
            Contract.Requires(toRegister != Guid.Empty);
            this.correlations.TryAdd(toRegister, toRegister);
        }

        public bool TryCorrelate(Guid toCorrelate)
        {
            Contract.Requires(toCorrelate != Guid.Empty);
            
            Guid temp;
            return this.correlations.TryRemove(toCorrelate, out temp);
        }
    }
}