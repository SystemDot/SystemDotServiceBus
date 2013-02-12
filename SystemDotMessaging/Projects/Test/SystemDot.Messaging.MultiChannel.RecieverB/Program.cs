using System;
using SystemDot.Esent;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Transport.Http.Configuration;

namespace SystemDot.Messaging.MultiChannel.RecieverB
{
    class Program
    {
        static void Main(string[] args)
        {
            IBus bus = Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowInfo = false })
                .UsingHttpTransport()
                .AsARemoteClientOf(MessageServer.Local())
                .UsingFilePersistence()
                .OpenChannel("TestRecieverB").ForRequestReplyRecieving()
                    .WithDurability()
                .Initialise();

            IocContainerLocator.Locate().Resolve<MessageHandlerRouter>().RegisterHandler(new MessageConsumer(bus));

            Console.WriteLine("I am reciever B");
            Console.ReadLine();                
        }
    }
}
