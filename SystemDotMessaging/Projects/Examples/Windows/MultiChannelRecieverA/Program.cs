﻿using System;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;

namespace MultiChannelRecieverA
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new IocContainer();
            container.RegisterFromAssemblyOf<Program>();

            Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowInfo = false })
                .ResolveReferencesWith(container)
                .RegisterHandlersFromContainer().BasedOn<IMessageConsumer>()
                .UsingFilePersistence()
                .UsingHttpTransport()
                    .AsAServer("ServerA")
                .OpenChannel("TestRecieverA").ForRequestReplyReceiving()
                    .WithDurability()
                    .Sequenced()
                .Initialise();

            Console.WriteLine("I am reciever A");
            Console.ReadLine();                
        }
    }
}
