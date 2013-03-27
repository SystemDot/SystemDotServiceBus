using System;
using System.Threading;
using SystemDot.Messaging.Test.Messages;

namespace SystemDot.Messaging.TestRequestReply.Sender
{
    public class MessageConsumer : IMessageConsumer
    {
        public void Handle(TestMessage message)
        {
            Console.WriteLine(
                "recieved reply {0} on thread {1}", 
                message.Text, 
                Thread.CurrentThread.ManagedThreadId);
        }
    }
}