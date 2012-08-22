using System;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Ioc;
using SystemDot.Messaging.Messages.Processing.Handling;
using SystemDot.Messaging.Test.Messages;

namespace SystemDot.Messaging.TestRequestReply.OtherSender
{
    class Program
    {
        static void Main(string[] args)
        {
            IBus bus = Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowInfo = false })
                .UsingHttpTransport(MessageServer.Local())
                .OpenChannel("TestOtherSender").ForRequestReplySendingTo("TestReciever")
                .Initialise();

            IocContainerLocator.Locate().Resolve<MessageHandlerRouter>().RegisterHandler(new MessageConsumer());

            do
            {
                Console.WriteLine("I am the other sender. Press enter to send messages..");
                string count = Console.ReadLine();
                
                Console.WriteLine("Sending messages");

                for (int i = 0; i < int.Parse(count); i++)
                {
                    
                bus.Send(new TestMessage("Other Hello" + i));
                bus.Send(new TestMessage("Other Hello1" + i));
                bus.Send(new TestMessage("Other Hello2" + i));
                bus.Send(new TestMessage("Other Hello3" + i));
                bus.Send(new TestMessage("Other Hello4" + i));
                bus.Send(new TestMessage("Other Hello5" + i));
                bus.Send(new TestMessage("Other Hello6" + i));
                bus.Send(new TestMessage("Other Hello7" + i));

                }
                
            }
            while (true);
        }
    }
}
