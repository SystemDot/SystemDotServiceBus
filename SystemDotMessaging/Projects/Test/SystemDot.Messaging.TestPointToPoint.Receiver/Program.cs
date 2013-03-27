using System;
using SystemDot.Esent;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Transport.Http.Configuration;
using SystemDot.Newtonsoft;

namespace SystemDot.Messaging.TestPointToPoint.Receiver
{
    class Program
    {
        static void Main(string[] args)
        {
            Configure.Messaging()
               .LoggingWith(new ConsoleLoggingMechanism { ShowDebug = false })
               .UsingFilePersistence()
               .UsingJsonSerialisation()
               .UsingHttpTransport()
               .AsAServer("ReceiverServer")
               .OpenChannel("TestReceive")
                   .ForPointToPointReceiving()
                   .WithDurability()
               .Initialise();

            IocContainerLocator.Locate().Resolve<MessageHandlerRouter>().RegisterHandler(new MessageConsumer());

            Console.WriteLine("I am the reciever. Press enter to exit");

            Console.ReadLine();
        }
    }
}
