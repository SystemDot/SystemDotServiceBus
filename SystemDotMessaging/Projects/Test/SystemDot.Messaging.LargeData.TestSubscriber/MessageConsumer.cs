using System;
using System.Threading;
using SystemDot.Messaging.LargeData.Messages;

namespace SystemDot.Messaging.LargeData.TestSubscriber
{
    public class MessageConsumer : IMessageConsumer
    {
        public void Handle(Channel1Message message)
        {
            Console.WriteLine(
                "recieved message for channel 1 {0} on thread {1}", 
                message.Number, 
                Thread.CurrentThread.ManagedThreadId);
        }

        public void Handle(Channel2Message message)
        {
            Console.WriteLine(
                "recieved message for channel 2 {0} on thread {1}",
                message.Number,
                Thread.CurrentThread.ManagedThreadId);
        }
    }
}