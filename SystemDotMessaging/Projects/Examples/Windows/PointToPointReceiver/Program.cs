using System;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;

namespace PointToPointReceiver
{
    class Program
    {
        static void Main(string[] args)
        {
            Configure.Messaging()
                .UsingHttpTransport()
                    .AsAServer("ReceiverServer")
                        .OpenChannel("PointToPointTest").ForPointToPointReceiving()
                            .RegisterHandlers(r => r.RegisterHandler(new TestMessageHandler()))
                 .Initialise();

            Console.WriteLine("I am the reciever. Press enter to exit");

            Console.ReadLine();
        }
    }
}
