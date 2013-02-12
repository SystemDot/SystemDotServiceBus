using System;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Transport.Http.Configuration;

namespace SystemDot.Messaging.TestRequestReply.Reciever
{
    class Program
    {
        static void Main(string[] args)
        {
            IBus bus = Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowDebug = true })
                .UsingHttpTransport()
                .AsARemoteClientOf(MessageServer.Local())
                .OpenChannel("TestReciever")
                    .ForRequestReplyRecieving()
                    .WithDurability()
                .Initialise();

            IocContainerLocator.Locate().Resolve<MessageHandlerRouter>().RegisterHandler(new MessageConsumer(bus));

            Console.WriteLine("I am the reciever. Press enter to exit");

            Console.ReadLine();
        }
    }
}
