using System;
using System.Threading;
using Messages;

namespace AuthenticationOtherRequestReplySender
{
    public class TestMessageHandler : IMessageHandler
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