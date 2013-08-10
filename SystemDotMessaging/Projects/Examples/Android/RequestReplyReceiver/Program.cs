using System;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;

namespace RequestReplyReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new IocContainer();
            container.RegisterFromAssemblyOf<Program>();

            Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowInfo = false, ShowDebug = false })
                .ResolveReferencesWith(container)
                .RegisterHandlersFromAssemblyOf<Program>()
                    .BasedOn<IMessageConsumer>()
                .UsingHttpTransport()
                    .AsAProxyFor("SenderServer")
                    .AsAServer("ReceiverServer")
                .OpenChannel("TestReply")
                    .ForRequestReplyReceiving()
                    .WithDurability()
                .Initialise();

            Console.WriteLine("I am the reciever. Press enter to exit");

            Console.ReadLine();
        }
    }

    
}
