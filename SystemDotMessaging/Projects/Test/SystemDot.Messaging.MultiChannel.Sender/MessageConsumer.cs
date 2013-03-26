using System;
using System.Threading;
using SystemDot.Messaging.Test.Messages;

namespace SystemDot.Messaging.MultiChannel.Sender
{
    public class MessageConsumer : IMessageConsumer
    {
        public void Handle(Channel1Reply message)
        {
            Console.WriteLine(
                "recieved reply {0} on thread {1}", 
                message.Text, 
                Thread.CurrentThread.ManagedThreadId);
        }

        public void Handle(Channel2Reply message)
        {
            Console.WriteLine(
                "recieved reply {0} on thread {1}",
                message.Text,
                Thread.CurrentThread.ManagedThreadId);
        }
    }
}