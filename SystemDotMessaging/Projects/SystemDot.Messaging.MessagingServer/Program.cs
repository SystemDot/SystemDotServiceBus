using System;
using SystemDot.Http.Builders;
using SystemDot.Logging;
using SystemDot.Messaging.Transport.Http.Remote.Servers.Configuration;
using SystemDot.Messaging.Transport.Http.Configuration;

namespace SystemDot.Messaging.MessagingServer
{
    class Program
    {
        static void Main(string[] args)
        {
            Configuration.Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowInfo = true, ShowDebug = true })
                .UsingHttpTransport()
                .AsARemoteServer()
                .Initialise();

            Logger.Info("I am the message server. Press enter to exit.");

            Console.ReadLine();
        }
    }
}
