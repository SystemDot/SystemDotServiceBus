using System;
using System.Threading;
using SystemDot.Messaging;
using Messages;

namespace AuthenticationRequestReplyReciever
{
    public class TestMessageHandler : IMessageHandler
    {
       public void Handle(TestMessage message)
        {
            Console.WriteLine(
                "recieved message {0} on thread {1} sending reply...", 
                message.Text, 
                Thread.CurrentThread.ManagedThreadId);

            Bus.Reply(new TestMessage("reply to " + message.Text));
        }
    }
}