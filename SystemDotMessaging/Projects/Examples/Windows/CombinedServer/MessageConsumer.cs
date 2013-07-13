using System;
using System.Threading;
using SystemDot.Messaging;
using Messages;

namespace CombinedServer
{
    public class MessageConsumer : IMessageConsumer
    {
        public void Handle(TestMessage message)
        {
            Console.WriteLine(
                "recieved message {0} on thread {1} sending reply...", 
                message.Text, 
                Thread.CurrentThread.ManagedThreadId);

            Bus.Reply(new TestMessage("reply to " + message.Text));
            Bus.Publish(new TestMessage("publish to " + message.Text));
        }
    }
}