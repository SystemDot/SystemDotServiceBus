using System;
using SystemDot.Messaging.Configuration;

namespace SystemDot.Messaging.TestRequestReply.Sender
{
    class Program
    {
        static void Main(string[] args)
        {
            Configure
                .RequestReplyServer()
                .OnChannelNamed("test")
                .SendingToLocalMachine();

            do
            {
                Console.WriteLine("Press a key to send message..");
                Console.ReadLine();
                Console.WriteLine("Sending message");
                MessageBus.Send("Hello");
                MessageBus.Send("Hello1");
                MessageBus.Send("Hello2");
                MessageBus.Send("Hello3");
                MessageBus.Send("Hello4");
                MessageBus.Send("Hello5");
                MessageBus.Send("Hello6");
                MessageBus.Send("Hello7");
            }
            while (true);
        }
    }
}
