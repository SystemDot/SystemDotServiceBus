using System;
using SystemDot.Esent;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Transport.Http.Configuration;

namespace SystemDot.Messaging.MultiChannel.RecieverA
{
    class Program
    {
        static void Main(string[] args)
        {
            IocContainerLocator.Locate().RegisterFromAssemblyOf<Program>();

            IBus bus = Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowInfo = false })
                .UsingHttpTransport()
                .AsAServer("ServerA")
                .UsingFilePersistence()
                .OpenChannel("TestRecieverA").ForRequestReplyRecieving()
                    .WithDurability()
                    .RegisterHandlersFromAssemblyOf<Program>()
                    .BasedOn<IMessageConsumer>()
                .Initialise();

            Console.WriteLine("I am reciever A");
            Console.ReadLine();                
        }
    }
}
