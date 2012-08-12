using System;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Messages.Handling;

namespace SystemDot.Messaging.Combined.Subscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.LoggingMechanism = new ConsoleLoggingMechanism();
            Logger.ShowInfo = false;

            Configure
                .UsingHttpMessaging()
                .WithLocalMessageServer()
                .OpenChannel("TestSubscriber").ForSubscribingTo("TestPublisher")
                .Initialise();

            IocContainer.Resolve<MessageHandlerRouter>().RegisterHandler(new MessageConsumer());
            
            Console.WriteLine("I am a subscriber, listening for messages..");

            Console.ReadLine();
        }
    }

   
}
