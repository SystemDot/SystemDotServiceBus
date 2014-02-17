using System;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging;
using SystemDot.Messaging.Configuration;
using Messages;

namespace RequestReplySender
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new IocContainer();
            container.RegisterFromAssemblyOf<Program>();

            Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism {ShowInfo = false, ShowDebug = false})
                //.UsingFilePersistence()
                .ResolveReferencesWith(container)
                .RegisterHandlersFromContainer().BasedOn<IMessageConsumer>()
                .UsingHttpTransport()
                    .AsAServer("SenderServer")
                .OpenChannel("TestRequest")
                    .ForRequestReplySendingTo("TestReply@ReceiverServer")
                    .WithDurability()
                    .Sequenced()
                .Initialise();

            do
            {
                Console.WriteLine("I am the sender. Press enter to send messages..");
                Console.ReadLine();

                Console.WriteLine("Sending messages");

                for (int i = 0; i < 2001; i++) Bus.Send(new TestMessage("Hello" + i));
            }
            while (true);
        }
    }
}
