using System;
using System.Threading;
using SystemDot.Messaging.Channels.Messages.Consuming;
using SystemDot.Messaging.Test.Messages;

namespace SystemDot.Messaging.TestRequestReply.Reciever
{
    public class MessageConsumer : IMessageHandler<TestMessage>
    {
        public void Handle(TestMessage message)
        {
            Console.WriteLine(
                "recieved message {0} on thread {1}", 
                message.Text, 
                Thread.CurrentThread.ManagedThreadId);
        }
    }
}