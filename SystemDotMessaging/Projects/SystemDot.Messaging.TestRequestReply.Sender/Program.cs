using System;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Test.Messages;

namespace SystemDot.Messaging.TestRequestReply.Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            Configure
                .Local()
                .ToPublishToChannel()
                .Initialise();

            do
            {
                Console.WriteLine("I am the sender. Press enter to send messages..");
                Console.ReadLine();
                
                Console.WriteLine("Sending message");

                MessageBus.Send(new TestMessage("Hello"));
                MessageBus.Send(new TestMessage("Hello1"));
                MessageBus.Send(new TestMessage("Hello2"));
                MessageBus.Send(new TestMessage("Hello3"));
                MessageBus.Send(new TestMessage("Hello4"));
                MessageBus.Send(new TestMessage("Hello5"));
                MessageBus.Send(new TestMessage("Hello6"));
                MessageBus.Send(new TestMessage("Hello7"));
            }
            while (true);
        }
    }
}
