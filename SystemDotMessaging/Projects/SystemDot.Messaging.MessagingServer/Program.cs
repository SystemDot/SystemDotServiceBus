using System;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Http;
using SystemDot.Messaging.Servers;
using SystemDot.Threading;

namespace SystemDot.Messaging.MessagingServer
{
    class Program
    {
        const string DefaultChannelName = "Default";

        static void Main(string[] args)
        {
            var coordinator = new ThreadedWorkCoordinator(new Threader());
            coordinator.RegisterWorker(BuildMessagingServer());
            BuildMessagingServer().StartWork();

            Console.Write("I am the message server. Press any key to exit.");
            Console.Read();
        }

        private static HttpServer BuildMessagingServer()
        {
            return new HttpServer(
                "http://localhost/" + DefaultChannelName + "/",
                new HttpMessagingServer(new BinaryFormatter(), null));
        }
    }
}
