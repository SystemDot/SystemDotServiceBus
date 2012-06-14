using System.Diagnostics.Contracts;
using SystemDot.Messaging.Pipes;
using SystemDot.Messaging.Recieving;
using SystemDot.Messaging.Servers;
using SystemDot.Threading;

namespace SystemDot.Messaging.Configuration.Remote
{
    public class MessageHandlerConfiguration
    {
        const string DefaultChannelName = "Default";

        readonly ThreadedWorkCoordinator workCoordinator;
        readonly ThreadPool threadPool;
        readonly IMessageHandler toRegister;

        public MessageHandlerConfiguration(
            ThreadedWorkCoordinator workCoordinator, 
            ThreadPool threadPool, 
            IMessageHandler toRegister)
        {
            Contract.Requires(workCoordinator != null);
            Contract.Requires(threadPool != null);
            Contract.Requires(toRegister != null);

            this.workCoordinator = workCoordinator;
            this.threadPool = threadPool;
            this.toRegister = toRegister;
        }

        public void Initialise()
        {
            var inputPipe = new MessagePump(this.threadPool);

            var server = new HttpServer("http://localhost/" + DefaultChannelName + "/", new MessageReciever(inputPipe));
            this.workCoordinator.RegisterWorker(server);

            var messageHandlerRouter = new MessageHandlerRouter(inputPipe);
            messageHandlerRouter.RegisterHandler(toRegister);

            this.workCoordinator.Start();
        }
    }
}