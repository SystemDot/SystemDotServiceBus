using System.Diagnostics.Contracts;
using SystemDot.Messaging.Pipes;
using SystemDot.Messaging.Sending;
using SystemDot.Threading;

namespace SystemDot.Messaging.Configuration.RequestReply.Server
{
    public class ChannelConfiguration
    {
        readonly string name;
        readonly ThreadedWorkCoordinator coordinator;
        readonly MessagePump outputPipe;

        public ChannelConfiguration(string name, ThreadedWorkCoordinator coordinator)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(coordinator != null);
            
            this.name = name;
            this.coordinator = coordinator;

            this.outputPipe = new MessagePump();
            this.coordinator.RegisterWorker(outputPipe);
        }

        public void SendingToLocalMachine()
        {
            MessageBus.Initialise(this.outputPipe); 
            new MessageSender(this.outputPipe, "http://localhost/" + this.name + "/");

            this.coordinator.Start();
        }
    }
}