using System;
using SystemDot.Esent;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Transport.Http.Configuration;

namespace SystemDot.Messaging.Combined.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new IocContainer();
            container.RegisterFromAssemblyOf<Program>();

            Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowInfo = false })
                .UsingFilePersistence()
                .UsingHttpTransport()
                .AsAServer("ReceiverPublisherServer")
                .OpenChannel("TestReceiver").ForRequestReplyRecieving()
                    .RegisterHandlersFromAssemblyOf<Program>()
                    .BasedOn<IMessageConsumer>()
                    .ResolveBy(container.Resolve)
                .OpenChannel("TestPublisher").ForPublishing()
                    .RegisterHandlersFromAssemblyOf<Program>()
                    .BasedOn<IMessageConsumer>()
                    .ResolveBy(container.Resolve)
                .Initialise();

            Console.WriteLine("I am the server. Press enter to exit");

            Console.ReadLine();
        }
    }
}
