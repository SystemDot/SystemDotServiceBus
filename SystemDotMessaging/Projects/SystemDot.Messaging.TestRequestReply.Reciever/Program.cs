using System;
using SystemDot.Messaging.Configuration;

namespace SystemDot.Messaging.TestRequestReply.Reciever
{
    class Program
    {
        static void Main(string[] args)
        {
            Configure
                .Remote()
                .UsingDefaults()
                .HandlingMessagesWith(new MessageConsumer())
                .Initialise();

            Console.WriteLine("I am the reciever. Press enter to exit");

            Console.ReadLine();
        }
    }
}
