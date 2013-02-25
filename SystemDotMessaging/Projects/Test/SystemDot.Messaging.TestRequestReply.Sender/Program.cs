using System;
using SystemDot.Esent;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Batching;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Test.Messages;
using SystemDot.Messaging.Transport.Http.Configuration;

namespace SystemDot.Messaging.TestRequestReply.Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            IBus bus = Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism {ShowInfo = false, ShowDebug = false})
                .UsingHttpTransport()
                .AsAServer("SenderServer")
                .UsingFilePersistence()
                .OpenChannel("TestRequest")
                .ForRequestReplySendingTo("TestReply@CHRIS-NEW-PC/ReceiverServer.CHRIS-NEW-PC/ReceiverServer")
                .WithDurability()
                .Initialise();

            IocContainerLocator.Locate().Resolve<MessageHandlerRouter>().RegisterHandler(new MessageConsumer());

            do
            {
                Console.WriteLine("I am the sender. Press enter to send messages..");
                Console.ReadLine();

                Console.WriteLine("Sending messages");

                for (int i = 0; i < 100; i++)
                {
                    bus.Send(new TestMessage("Hello" + i)); 
                }

            } while (true);
        }
    }
}
