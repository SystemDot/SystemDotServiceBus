using System;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;

namespace DirectRequestReplyReciever
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new IocContainer();
            container.RegisterFromAssemblyOf<Program>();

            Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowDebug = false })
                .ResolveReferencesWith(container)
                .RegisterHandlersFromAssemblyOf<Program>()
                    .BasedOn<IMessageConsumer>()
                .UsingHttpTransport()
                    .AsAServer("ReceiverServer")
                .OpenDirectChannel("TestReply")
                    .ForRequestReplyReceiving()
                .Initialise();

            Console.WriteLine("I am the reciever. Press enter to exit");

            Console.ReadLine();
        }
    }

    
}
