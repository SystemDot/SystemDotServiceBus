﻿using System;
using System.Threading;
using SystemDot.Messaging.Recieving;

namespace SystemDot.Messaging.TestRequestReply.Reciever
{
    public class MessageConsumer : IMessageHandler<string>
    {
        public void Handle(string message)
        {
            Console.WriteLine(
                "recieved message {0} on thread {1}", 
                message, 
                Thread.CurrentThread.ManagedThreadId);
        }
    }
}