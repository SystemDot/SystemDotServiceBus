﻿using System;
using SystemDot.Esent;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Transport.Http.Configuration;

namespace SystemDot.Messaging.Combined.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            IBus bus = Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowInfo = false })
                .UsingHttpTransport()
                .AsAServer("ReceiverPublisherServer")
                .UsingFilePersistence()
                .OpenChannel("TestReceiver").ForRequestReplyRecieving()
                .OpenChannel("TestPublisher").ForPublishing()
                .Initialise();

            IocContainerLocator.Locate().Resolve<MessageHandlerRouter>().RegisterHandler(new MessageConsumer(bus));

            Console.WriteLine("I am the server. Press enter to exit");

            Console.ReadLine();
        }
    }
}
