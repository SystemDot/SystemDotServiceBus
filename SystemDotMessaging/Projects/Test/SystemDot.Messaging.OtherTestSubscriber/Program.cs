using System;
using SystemDot.Esent;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Transport.Http.Configuration;

namespace SystemDot.Messaging.OtherTestSubscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.LoggingMechanism = new ConsoleLoggingMechanism { ShowDebug = false };

            Configure.Messaging()
                .UsingHttpTransport()
                .AsARemoteClientOf(MessageServer.Local())
                .UsingFilePersistence()
                .OpenChannel("TestOtherSubscriber")
                    .ForSubscribingTo("TestPublisher")
                    .WithDurability()
                .Initialise();

            IocContainerLocator.Locate().Resolve<MessageHandlerRouter>().RegisterHandler(new MessageConsumer());
            
            Console.WriteLine("I am the other subscriber, listening for messages..");

            Console.ReadLine();
        }
    }

   
}
