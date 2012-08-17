using System;
using System.Threading;
using SystemDot.Messaging.Test.Messages;

namespace SystemDot.Messaging.Combined.Subscriber
{
    public class MessageConsumer
    {
        public void Handle(TestMessage message)
        {
            Console.WriteLine(
                "recieved message {0} on thread {1}", 
                message.Text, 
                Thread.CurrentThread.ManagedThreadId);
        }
    }
}