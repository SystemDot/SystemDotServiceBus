using System;
using System.Threading;
using SystemDot.Messaging;
using Messages;

namespace DirectRequestReplySender
{
    public class DirectSender
    {
        public void Send()
        {
            Bus.SendDirect(new TestMessage("Hello"), this, OnServerError);
            Bus.SendDirect(new TestMessage("Hello1"), this, OnServerError);
            Bus.SendDirect(new TestMessage("Hello2"), this, OnServerError);
            Bus.SendDirect(new TestMessage("Hello3"), this, OnServerError);
            Bus.SendDirect(new TestMessage("Hello4"), this, OnServerError);
            Bus.SendDirect(new TestMessage("Hello5"), this, OnServerError);
            Bus.SendDirect(new TestMessage("Hello6"), this, OnServerError);
            Bus.SendDirect(new TestMessage("Hello7"), this, OnServerError); 
        }

        public void Handle(TestMessage message)
        {
            Console.WriteLine(
                "recieved reply {0} on thread {1}",
                message.Text,
                Thread.CurrentThread.ManagedThreadId);
        }

        static void OnServerError(Exception exception)
        {
            Console.WriteLine("Could not contact server {0}", exception.Message);
        }
    }
}