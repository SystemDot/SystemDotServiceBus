using System;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Messages.Consuming;

namespace SystemDot.Messaging.TestRequestReply.Reciever
{
    class Program
    {
        static void Main(string[] args)
        {
            IBus bus = Configure.WithLocalMessageServer()
                .OpenChannel("TestReciever")
                .AsRequestReplyReciever()
                .Initialise();

            IocContainer.Resolve<MessageHandlerRouter>().RegisterHandler(new MessageConsumer(bus));

            Console.WriteLine("I am the reciever. Press enter to exit");

            Console.ReadLine();
        }
    }
}
