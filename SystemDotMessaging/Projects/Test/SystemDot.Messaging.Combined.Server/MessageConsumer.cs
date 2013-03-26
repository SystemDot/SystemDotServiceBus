using System;
using System.Threading;
using SystemDot.Messaging.Test.Messages;

namespace SystemDot.Messaging.Combined.Server
{
    public class MessageConsumer : IMessageConsumer
    {
        readonly IBus bus;

        public MessageConsumer(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(TestMessage message)
        {
            Console.WriteLine(
                "recieved message {0} on thread {1} sending reply...", 
                message.Text, 
                Thread.CurrentThread.ManagedThreadId);

            this.bus.Reply(new TestMessage("reply to " + message.Text));
            this.bus.Publish(new TestMessage("publish to " + message.Text));
        }
    }
}