﻿using System;
using SystemDot.Esent;
using SystemDot.Ioc;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Transport.Http.Configuration;

namespace SystemDot.Messaging.MultiChannel.RecieverA
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new IocContainer();
            container.RegisterFromAssemblyOf<Program>();

            Configure.Messaging()
                .LoggingWith(new ConsoleLoggingMechanism { ShowInfo = false })
                .RegisterHandlersFromAssemblyOf<Program>()
                    .BasedOn<IMessageConsumer>()
                    .ResolveBy(container.Resolve)
                .UsingFilePersistence()
                .UsingHttpTransport()
                    .AsAServer("ServerA")
                .OpenChannel("TestRecieverA").ForRequestReplyRecieving()
                    .WithDurability()
                .Initialise();

            Console.WriteLine("I am reciever A");
            Console.ReadLine();                
        }
    }
}
