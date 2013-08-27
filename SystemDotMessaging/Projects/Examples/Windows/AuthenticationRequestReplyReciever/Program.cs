using System;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;
using Messages;

namespace AuthenticationRequestReplyReciever
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
                    .AsAServer("ReceiverServer")
                    .RequiresAuthentication()
                        .AcceptsRequest<AuthenticationRequest>()
                        .AuthenticatesOnReply<AuthenticatedResponse>()
                        .ExpiresAfter(TimeSpan.FromMinutes(1))
                        .OnExpiry(s => Console.WriteLine("Session expired: {0}", s))
                .OpenChannel("TestReply")
                    .ForRequestReplyReceiving()
                    .WithDurability()
                .Initialise();

            Console.WriteLine("I am the reciever. Press enter to exit");

            Console.ReadLine();
        }
    }
}
