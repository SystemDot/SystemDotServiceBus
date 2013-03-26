using System;
using System.Threading;
using SystemDot.Messaging.Test.Messages;

namespace SystemDot.Messaging.MultiChannel.RecieverA
{
    public class MessageConsumer : IMessageConsumer
    {
        readonly IBus bus;

        public MessageConsumer(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(Channel1Request message)
        {
            Console.WriteLine(
                "recieved send {0} on thread {1}", 
                message.Text, 
                Thread.CurrentThread.ManagedThreadId);

            bus.Reply(new Channel1Reply("Reply to " + message.Text));
        }
    }
}