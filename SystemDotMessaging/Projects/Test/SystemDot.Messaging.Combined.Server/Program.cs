using System;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Messages.Handling;

namespace SystemDot.Messaging.Combined.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.LoggingMechanism = new ConsoleLoggingMechanism();
            Logger.ShowInfo = false;

            IBus bus = Configure
                .UsingHttpMessaging()
                    .WithLocalMessageServer()                
                        .OpenChannel("TestReciever").ForRequestReplyRecieving()
                        .OpenChannel("TestPublisher").ForPublishing()
                .Initialise();

            IocContainer.Resolve<MessageHandlerRouter>().RegisterHandler(new MessageConsumer(bus));

            Console.WriteLine("I am the server. Press enter to exit");

            Console.ReadLine();
        }
    }
}
