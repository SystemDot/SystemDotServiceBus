using System;
using System.Threading;
using SystemDot.Messaging.Test.Messages;

namespace SystemDot.Messaging.MultiChannel.RecieverB
{
    public class MessageConsumer : IMessageConsumer
    {
        public void Handle(Channel2Request message)
        {
            Console.WriteLine(
                "recieved send {0} on thread {1}", 
                message.Text, 
                Thread.CurrentThread.ManagedThreadId);

            Bus.Reply(new Channel2Reply("Reply to " + message.Text));
        }
    }
}