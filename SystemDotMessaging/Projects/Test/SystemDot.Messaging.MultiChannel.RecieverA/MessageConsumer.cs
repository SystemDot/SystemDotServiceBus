using System;
using System.Threading;
using SystemDot.Messaging.Test.Messages;

namespace SystemDot.Messaging.MultiChannel.RecieverA
{
    public class MessageConsumer : IMessageConsumer
    {
        public void Handle(Channel1Request message)
        {
            Console.WriteLine(
                "recieved send {0} on thread {1}", 
                message.Text, 
                Thread.CurrentThread.ManagedThreadId);

            Bus.Reply(new Channel1Reply("Reply to " + message.Text));
        }
    }
}