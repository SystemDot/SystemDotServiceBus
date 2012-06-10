using System.Diagnostics.Contracts;
using SystemDot.Messaging.Pipes;
using SystemDot.Messaging.Recieving;
using SystemDot.Threading;

namespace SystemDot.Messaging.Configuration.RequestReply.Client
{
    public class ChannelConfiguration
    {
        readonly string name;
        readonly ThreadedWorkCoordinator coordinator;
        readonly MessagePump inputPipe;

        public ChannelConfiguration(string name, ThreadedWorkCoordinator coordinator)
        {
            Contract.Requires(!string.IsNullOrEmpty(name));
            Contract.Requires(coordinator != null);
            
            this.name = name;
            this.coordinator = coordinator;
            
            this.inputPipe = new MessagePump();
            this.coordinator.RegisterWorker(inputPipe);
        }

        public ConsumerConfiguration OnLocalMachine()
        {
            this.coordinator.RegisterWorker(new MessageReciever(this.inputPipe, "http://localhost/" + this.name + "/"));
            var broadcaster = new ConsumerMessageBroadcaster(this.inputPipe);
            
            this.coordinator.Start();

            return new ConsumerConfiguration(inputPipe, broadcaster);
        }
    }
}