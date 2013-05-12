using System;
using SystemDot.Esent;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Transport.Http.Configuration;
using SystemDot.Newtonsoft;

namespace SystemDot.Messaging.TestReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new IocContainer();
            container.RegisterFromAssemblyOf<Program>();

            Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowDebug = false })
                .RegisterHandlersFromAssemblyOf<Program>()
                    .BasedOn<IMessageConsumer>()
                    .ResolveBy(container.Resolve)
                .UsingFilePersistence()
                .UsingJsonSerialisation()
                .UsingHttpTransport()
                    .AsARemoteServer("MetroProxy")
                    .AsAServer("ReceiverServer")
                .OpenChannel("TestReply")
                    .ForRequestReplyRecieving()
                    .WithDurability()
                .Initialise();

            Console.WriteLine("I am the reciever. Press enter to exit");

            Console.ReadLine();
        }
    }

    
}
