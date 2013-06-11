using System;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Test.Messages;

namespace SystemDot.Messaging.TestRequestReply.Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new IocContainer();
            container.RegisterFromAssemblyOf<Program>();

            Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism {ShowInfo = false, ShowDebug = false})
                .ResolveReferencesWith(container)
                .RegisterHandlersFromAssemblyOf<Program>()
                    .BasedOn<IMessageConsumer>()
                .UsingJsonSerialisation()
                .UsingHttpTransport()
                    .AsAServer("SenderServer")
                .OpenChannel("TestRequest")
                    .ForRequestReplySendingTo("TestReply@ReceiverServer")
                    .WithDurability()
                .Initialise();

            do
            {
                Console.WriteLine("I am the sender. Press enter to send messages..");
                Console.ReadLine();

                Console.WriteLine("Sending messages");

                Bus.Send(new TestMessage("Hello"));
                Bus.Send(new TestMessage("Hello1"));
                Bus.Send(new TestMessage("Hello2"));
                Bus.Send(new TestMessage("Hello3"));
                Bus.Send(new TestMessage("Hello4"));
                Bus.Send(new TestMessage("Hello5"));
                Bus.Send(new TestMessage("Hello6"));
                Bus.Send(new TestMessage("Hello7"));        
            }
            while (true);
        }
    }
}
