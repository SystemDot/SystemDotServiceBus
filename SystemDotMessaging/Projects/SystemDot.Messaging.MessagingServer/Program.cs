using System;
using SystemDot.Http.Builders;
using SystemDot.Logging;
using SystemDot.Messaging.Transport.Http.LongPolling.Servers.Builders;

namespace SystemDot.Messaging.MessagingServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.LoggingMechanism = new ConsoleLoggingMechanism { ShowInfo = true, ShowDebug = true };

            new HttpRemoteTransportBuilder(new HttpServerBuilder()).Build();

            Logger.Info("I am the message server. Press enter to exit.");

            Console.ReadLine();
        }
    }
}
