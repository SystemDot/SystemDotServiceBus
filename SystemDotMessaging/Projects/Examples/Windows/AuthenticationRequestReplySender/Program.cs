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
                .LoggingWith(new ConsoleLoggingMechanism { ShowDebug = false })
                .ResolveReferencesWith(container)
                .RegisterHandlersFromAssemblyOf<Program>()
                    .BasedOn<IMessageHandler>()
                .UsingHttpTransport()
                    .AsAServer("SenderServer")
                    .AuthenticateToServer("ReceiverServer")
                        .WithRequest<AuthenticationRequest>()
                        .OnExpiry(s => Console.WriteLine("Session expired: {0}", s))
                .OpenChannel("TestRequest")
                    .ForRequestReplySendingTo("TestReply@ReceiverServer")
                    .WithDurability()
                .Initialise();


            Console.WriteLine("I am the sender. Whats the password? (its 'Hello')");
            string password = Console.ReadLine();

            Bus.SendDirect(new AuthenticationRequest { Password = password }, new AuthenticationHandler(), exception => { });

            do
            {
                Console.WriteLine("I am the sender. Press enter to send messages..");
                Console.ReadLine();

                Console.WriteLine("Sending messages");

                Bus.Send(new TestMessage("Hello"));
            } while (true);
        }

        class AuthenticationHandler
        {
            public void Handle(AuthenticatedResponse message)
            {
                Console.WriteLine("Authenticated to server");
            }

            public void Handle(AuthenticationFailedResponse message)
            {
                Console.WriteLine("Failed authentication to server");
            }
        }
    }

}
