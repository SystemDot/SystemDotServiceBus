using System;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Test.Messages;

namespace SystemDot.Messaging.TestRequestReply.Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                Console.WriteLine("I am the sender. Press enter to send messages..");
                Console.ReadLine();
                
                Console.WriteLine("Sending message");

                MessageBus.SendMessage(new TestMessage("Hello"));
                MessageBus.SendMessage(new TestMessage("Hello1"));
                MessageBus.SendMessage(new TestMessage("Hello2"));
                MessageBus.SendMessage(new TestMessage("Hello3"));
                MessageBus.SendMessage(new TestMessage("Hello4"));
                MessageBus.SendMessage(new TestMessage("Hello5"));
                MessageBus.SendMessage(new TestMessage("Hello6"));
                MessageBus.SendMessage(new TestMessage("Hello7"));
            }
            while (true);
        }
    }
}
