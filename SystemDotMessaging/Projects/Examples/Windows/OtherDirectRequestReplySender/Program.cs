using System;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging;
using SystemDot.Messaging.Configuration;
using Messages;

namespace OtherDirectRequestReplySender
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
                .RegisterHandlersFromContainer().BasedOn<IMessageConsumer>()
                .UsingHttpTransport()
                    .AsAServer("OtherSenderServer")
                .OpenDirectChannel("OtherTestRequest")
                    .ForRequestReplySendingTo("TestReply@ReceiverServer")
                .Initialise();

            do
            {
                Console.WriteLine("I am the other sender. Press enter to send messages..");
                Console.ReadLine();
                Console.WriteLine("Sending messages");

                Bus.SendDirect(new TestMessage("Hello"));
                Bus.SendDirect(new TestMessage("Hello1"));
                Bus.SendDirect(new TestMessage("Hello2"));
                Bus.SendDirect(new TestMessage("Hello3"));
                Bus.SendDirect(new TestMessage("Hello4"));
                Bus.SendDirect(new TestMessage("Hello5"));
                Bus.SendDirect(new TestMessage("Hello6"));
                Bus.SendDirect(new TestMessage("Hello7")); 
            }
            while (true);
        }
    }
}
