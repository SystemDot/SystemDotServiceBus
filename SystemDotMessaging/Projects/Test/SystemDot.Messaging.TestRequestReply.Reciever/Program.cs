﻿using System;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Configuration.HttpMessaging;
using SystemDot.Messaging.Messages.Processing.Handling;

namespace SystemDot.Messaging.TestRequestReply.Reciever
{
    class Program
    {
        static void Main(string[] args)
        {
            IBus bus = Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowInfo = false })
                .UsingHttpTransport(MessageServer.Local())
                .OpenChannel("TestReciever")
                .ForRequestReplyRecieving()
                .Initialise();

            IocContainer.Resolve<MessageHandlerRouter>().RegisterHandler(new MessageConsumer(bus));

            Console.WriteLine("I am the reciever. Press enter to exit");

            Console.ReadLine();
        }
    }
}
