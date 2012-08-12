using System;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Messages.Handling;

namespace SystemDot.Messaging.TestRequestReply.Reciever
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
                .OpenChannel("TestReciever")
                .ForRequestReplyRecieving()
                .Initialise();

            IocContainer.Resolve<MessageHandlerRouter>().RegisterHandler(new MessageConsumer(bus));

            Console.WriteLine("I am the reciever. Press enter to exit");

            Console.ReadLine();
        }
    }
}
