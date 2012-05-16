using System;
using SystemDot.Messaging.Configuration;

namespace SystemDot.Messaging.TestSubscriber
{
    class Program
    {
        static void Main(string[] args)
        {
            Configuration.Configuration
                .ListeningOn("http://localhost/test/client/")
                .WorkerThreads(4)
                .OpenChannel("Test")
                    .ConsumeMessages().With(new MessageConsumer())
                    .Build();
            
            Console.WriteLine("Started listening..");

            Console.ReadLine();
        }
    }
}
