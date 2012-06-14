using System.Diagnostics.Contracts;
using SystemDot.Messaging.Configuration.Remote;
using SystemDot.Threading;

namespace SystemDot.Messaging.Configuration.Local
{
    public class LocalConfiguration
    {
        readonly ThreadPool threadPool;

        public LocalConfiguration(ThreadPool threadPool)
        {
            Contract.Requires(threadPool != null);

            this.threadPool = threadPool;
        }

        public UsingDefaultsConfiguration UsingDefaults()
        {
            return new UsingDefaultsConfiguration(this.threadPool);
        }
    }
}