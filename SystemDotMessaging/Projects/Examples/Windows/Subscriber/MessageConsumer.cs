using System;
using System.Threading;
using Messages;

namespace Subscriber
{
    public class MessageConsumer : IMessageConsumer
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