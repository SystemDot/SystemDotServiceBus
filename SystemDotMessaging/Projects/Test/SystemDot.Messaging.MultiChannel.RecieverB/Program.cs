using System;
using SystemDot.Esent;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Transport.Http.Configuration;

namespace SystemDot.Messaging.MultiChannel.RecieverB
{
    class Program
    {
        static void Main(string[] args)
        {
            IocContainerLocator.Locate().RegisterFromAssemblyOf<Program>();

            IBus bus = Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowInfo = false })
                .UsingHttpTransport()
                .AsAServer("ServerB")
                .UsingFilePersistence()
                .OpenChannel("TestRecieverB").ForRequestReplyRecieving()
                    .WithDurability()
                    .RegisterHandlersFromAssemblyOf<Program>()
                    .BasedOn<IMessageConsumer>()
                .Initialise();

            Console.WriteLine("I am reciever B");
            Console.ReadLine();                
        }
    }
}
