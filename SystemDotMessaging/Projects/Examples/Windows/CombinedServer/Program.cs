using System;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;

namespace CombinedServer
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new IocContainer();
            container.RegisterFromAssemblyOf<Program>();

            Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowInfo = false })
                .ResolveReferencesWith(container)
                .RegisterHandlersFromAssemblyOf<Program>()
                    .BasedOn<IMessageConsumer>()
                .UsingFilePersistence()
                .UsingHttpTransport()
                .AsAServer("ReceiverPublisherServer")
                    .OpenChannel("TestReceiver").ForRequestReplyReceiving()
                    .OpenChannel("TestPublisher").ForPublishing()
                .Initialise();

            Console.WriteLine("I am the server. Press enter to exit");

            Console.ReadLine();
        }
    }
}
