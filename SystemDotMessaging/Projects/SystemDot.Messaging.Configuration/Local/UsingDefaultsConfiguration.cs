using System.Diagnostics.Contracts;
using SystemDot.Messaging.Pipes;
using SystemDot.Messaging.Sending;
using SystemDot.Threading;

namespace SystemDot.Messaging.Configuration.Local
{
    public class UsingDefaultsConfiguration
    {
        const string DefaultChannelName = "Default";

        readonly ThreadPool threadPool;

        public UsingDefaultsConfiguration(ThreadPool threadPool)
        {
            Contract.Requires(threadPool != null);

            this.threadPool = threadPool;
        }

        public void Initialise()
        {
            var outputPipe = new MessagePump(this.threadPool);
            MessageBus.Initialise(outputPipe);
            new MessageSender(outputPipe, "http://localhost/" + DefaultChannelName + "/");
        }
    }
}