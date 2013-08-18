using System;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging;
using SystemDot.Messaging.Batching;
using SystemDot.Messaging.Configuration;
using Messages;

namespace AuthenticationOtherRequestReplySender
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new IocContainer();
            container.RegisterFromAssemblyOf<Program>();

            Configure.Messaging()
                .ResolveReferencesWith(container)
                .RegisterHandlersFromAssemblyOf<Program>()
                    .BasedOn<IMessageHandler>()
                .UsingHttpTransport()
                    .AsAServer("OtherSenderServer")
                    .AuthenticateToServer("ReceiverServer")
                        .WithRequest<AuthenticationRequest>()
                .OpenChannel("OtherTestRequest")
                    .ForRequestReplySendingTo("TestReply@ReceiverServer")
                    .WithDurability()
                .Initialise();


            Console.WriteLine("I am the sender. Whats the password? (its 'Hello')");
            string password = Console.ReadLine();

            Bus.SendDirect(new AuthenticationRequest {Password = password});

            do
            {
                Console.WriteLine("I am the sender. Press enter to send messages..");
                Console.ReadLine();

                Console.WriteLine("Sending messages");

                using (Batch batch = Bus.BatchSend())
                {
                    Bus.Send(new TestMessage("Hello"));
                    Bus.Send(new TestMessage("Hello1"));
                    Bus.Send(new TestMessage("Hello2"));
                    Bus.Send(new TestMessage("Hello3"));
                    Bus.Send(new TestMessage("Hello4"));
                    Bus.Send(new TestMessage("Hello5"));
                    Bus.Send(new TestMessage("Hello6"));
                    Bus.Send(new TestMessage("Hello7"));

                    batch.Complete();
                }
            } while (true);
        }
    }
}
