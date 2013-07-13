using System;
using System.Threading;
using Messages;

namespace MultiChannelSender
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