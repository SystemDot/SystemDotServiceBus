using System;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Messages.Processing.Handling;
using SystemDot.Messaging.Test.Messages;

namespace SystemDot.Messaging.MultiChannel.Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            IBus bus = Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowInfo = false })
                .UsingHttpTransport(MessageServer.Local())
                .OpenChannel("TestSenderA").ForRequestReplySendingTo("TestRecieverA").OnlyForMessages(FilteredBy.NamePattern("Channel1"))
                .OpenChannel("TestSenderB").ForRequestReplySendingTo("TestRecieverB").OnlyForMessages(FilteredBy.NamePattern("Channel2"))
                .Initialise();

            IocContainerLocator.Locate().Resolve<MessageHandlerRouter>().RegisterHandler(new MessageConsumer());

            do
            {
                Console.WriteLine("I am the sender. Press A to send messages to reciever A, press B to send messages to reciever B..");
                ConsoleKeyInfo key = Console.ReadKey();

                if (key.Key == ConsoleKey.A)
                {
                    Console.WriteLine("Sending messages to Channel A");

                    bus.Send(new Channel1Request("Channel A Hello"));
                    bus.Send(new Channel1Request("Channel A Hello1"));
                    bus.Send(new Channel1Request("Channel A Hello2"));
                    bus.Send(new Channel1Request("Channel A Hello3"));
                    bus.Send(new Channel1Request("Channel A Hello4"));
                    bus.Send(new Channel1Request("Channel A Hello5"));
                    bus.Send(new Channel1Request("Channel A Hello6"));
                    bus.Send(new Channel1Request("Channel A Hello7"));
                }

                if (key.Key == ConsoleKey.B)
                {
                    Console.WriteLine("Sending messages to Channel B");

                    bus.Send(new Channel2Request("Channel B Hello"));
                    bus.Send(new Channel2Request("Channel B Hello1"));
                    bus.Send(new Channel2Request("Channel B Hello2"));
                    bus.Send(new Channel2Request("Channel B Hello3"));
                    bus.Send(new Channel2Request("Channel B Hello4"));
                    bus.Send(new Channel2Request("Channel B Hello5"));
                    bus.Send(new Channel2Request("Channel B Hello6"));
                    bus.Send(new Channel2Request("Channel B Hello7"));
                }
            }
            while (true);
        }
    }
}
