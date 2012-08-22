
using System;
using SystemDot.Messaging.Test.Messages;

namespace SystemDot.Messaging.TestReciever
{
    public class RequestHandler
    {
        readonly IBus bus;
        
        public RequestHandler(IBus bus)
        {
            this.bus = bus;
        }

        public void Handle(TestQuery message)
        {
            bus.Reply(new TestResponse { Text = "Reply to " +  message.Text + " at " + DateTime.Now });
        }
    }
}