﻿using System;
using SystemDot.Logging;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Messages.Handling;
using SystemDot.Messaging.Test.Messages;

namespace SystemDot.Messaging.Combined.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Logger.LoggingMechanism = new ConsoleLoggingMechanism();
            Logger.ShowInfo = false;

            IBus bus = Configure
                .UsingHttpMessaging()
                .WithLocalMessageServer()
                .OpenChannel("TestSender").ForRequestReplySending("TestReciever")
                .Initialise();

            IocContainer.Resolve<MessageHandlerRouter>().RegisterHandler(new MessageConsumer());

            do
            {
                Console.WriteLine("I am the client. Press enter to send messages..");
                Console.ReadLine();
                
                Console.WriteLine("Sending messages");

                bus.Send(new TestMessage("Hello"));
                bus.Send(new TestMessage("Hello1"));
                bus.Send(new TestMessage("Hello2"));
                bus.Send(new TestMessage("Hello3"));
                bus.Send(new TestMessage("Hello4"));
                bus.Send(new TestMessage("Hello5"));
                bus.Send(new TestMessage("Hello6"));
                bus.Send(new TestMessage("Hello7"));
            }
            while (true);
        }
    }
}
