using System;
using System.Collections.Generic;
using SystemDot.Messaging.Channels.Recieving.Http;

namespace SystemDot.Messaging.Specifications.channels.receiving
{
    public class TestMessageListener : IMessageListener 
    {
        readonly List<string> messages;
        
        public TestMessageListener(params string[] messages)
        {
            this.messages = new List<string>(messages);
        }

        public bool IsRunning { get; private set; }
        
        public void Start(Action<object> onMessageRecieved)
        {
            this.messages.ForEach(onMessageRecieved);
            this.IsRunning = true;
        }

        public void Stop()
        {
            this.IsRunning = false;
        }
    }
}