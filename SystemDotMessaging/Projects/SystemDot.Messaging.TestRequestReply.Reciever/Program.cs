using System;
using SystemDot.Messaging.Configuration;

namespace SystemDot.Messaging.TestRequestReply.Reciever
{
    class Program
    {
        static void Main(string[] args)
        {
            Configure
                .RequestReplyClient()
                .OnChannelNamed("test")
                .OnLocalMachine()
                .ConsumingMessagesWith(new MessageConsumer());

            Console.WriteLine("Started listening..");

            Console.ReadLine();
        }
    }
}
