using System;
using System.Threading;
using SystemDot.Messaging.Channels.Recieving;

namespace SystemDot.Messaging.TestSubscriber
{
    public class MessageConsumer : IConsume<string>
    {
        public void Consume(string message)
        {
            Console.WriteLine(
                "recieved message {0} on thread {1}", 
                message, 
                Thread.CurrentThread.ManagedThreadId);
        }
    }
}