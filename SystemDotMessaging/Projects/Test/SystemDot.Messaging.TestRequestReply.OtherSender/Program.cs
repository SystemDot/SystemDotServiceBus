using System;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Batching;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Pipelines;
using SystemDot.Messaging.Test.Messages;
using SystemDot.Messaging.Transport.Http.Configuration;

namespace SystemDot.Messaging.TestRequestReply.OtherSender
{
    class Program
    {
        static void Main(string[] args)
        {
            IBus bus = Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowInfo = false, ShowDebug = false })
                .UsingHttpTransport()
                .AsAServer("OtherSenderServer")
                .OpenChannel("OtherTestRequest")
                    .ForRequestReplySendingTo("TestReply@/ReceiverServer")
                    .WithDurability()
                .Initialise();

            IocContainerLocator.Locate().Resolve<MessageHandlerRouter>().RegisterHandler(new MessageConsumer());

            do
            {
                Console.WriteLine("I am the sender. Press enter to send messages..");
                Console.ReadLine();

                Console.WriteLine("Sending messages");

                using (Batch batch = bus.BatchSend())
                {
                    bus.Send(new TestMessage("Hello"));
                    bus.Send(new TestMessage("Hello1"));
                    bus.Send(new TestMessage("Hello2"));
                    bus.Send(new TestMessage("Hello3"));
                    bus.Send(new TestMessage("Hello4"));
                    bus.Send(new TestMessage("Hello5"));
                    bus.Send(new TestMessage("Hello6"));
                    bus.Send(new TestMessage("Hello7"));

                    batch.Complete();
                }
            }
            while (true);
        }
    }
}
