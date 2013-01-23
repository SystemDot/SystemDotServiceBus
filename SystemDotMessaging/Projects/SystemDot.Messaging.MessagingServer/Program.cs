using System;
using SystemDot.Logging;
using SystemDot.Messaging.Transport.Http.LongPolling.Servers.Builders;

namespace SystemDot.Messaging.MessagingServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.LoggingMechanism = new ConsoleLoggingMechanism { ShowInfo = true, ShowDebug = true };

            MessagingServerBuilder.Build().Start();

            Logger.Info("I am the message server. Press enter to exit.");

            Console.ReadLine();
        }
    }
}
