using System;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Storage.Sql;

namespace SystemDot.Messaging.TestRequestReply.Reciever
{
    class Program
    {
        static void Main(string[] args)
        {
            IBus bus = Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowInfo = false })
                .UsingHttpTransport(MessageServer.Local())
                .UsingSqlPersistence()
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
