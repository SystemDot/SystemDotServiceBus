using System;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging;
using SystemDot.Messaging.Configuration;
using Messages;

namespace AuthenticationRequestReplySender
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
                    .AsAServer("SenderServer")
                    .AuthenticateToServer("ReceiverServer")
                        .WithRequest<AuthenticationRequest>()
                .OpenChannel("TestRequest")
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

                Bus.Send(new TestMessage("Hello"));
                Bus.Send(new TestMessage("Hello1"));
                Bus.Send(new TestMessage("Hello2"));
                Bus.Send(new TestMessage("Hello3"));
                Bus.Send(new TestMessage("Hello4"));
                Bus.Send(new TestMessage("Hello5"));
                Bus.Send(new TestMessage("Hello6"));
                Bus.Send(new TestMessage("Hello7"));
            } while (true);
        }
    }
}
