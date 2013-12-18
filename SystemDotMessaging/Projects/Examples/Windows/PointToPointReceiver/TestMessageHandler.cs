using System;
using Messages;

namespace PointToPointReceiver
{
    public class TestMessageHandler
    {
        public void Handle(TestMessage message)
        {
            Console.WriteLine("recieved message {0}", message.Text);
        }
    }
}