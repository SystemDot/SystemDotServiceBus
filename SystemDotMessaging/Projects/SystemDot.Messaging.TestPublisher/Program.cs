using System;
using SystemDot.Messaging.Configuration;

namespace SystemDot.Messaging.TestPublisher
{
    class Program
    {
        static void Main(string[] args)
        {
            IBus bus = Configure
                .WithLocalMessageServer()
                .AsPublisher("TestPublisher")
                .Initialise();
            
            do
            {
                Console.WriteLine("Press a key to send message..");
                Console.ReadLine();
                Console.WriteLine("Sending message");
                bus.Send("Hello");
                bus.Send("Hello1");
                bus.Send("Hello2");
                bus.Send("Hello3");
                bus.Send("Hello4");
                bus.Send("Hello5");
                bus.Send("Hello6");
                bus.Send("Hello7");
            }
            while (true);
        }
    }
}
