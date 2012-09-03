using System;
using SystemDot.Ioc;

namespace SystemDot.Messaging.Configuration
{
    public class MessageServer
    {
        public static MessageServer Local()
        {
            return new MessageServer(Environment.MachineName); 
        }

        public static MessageServer Named(string name)
        {
            return new MessageServer(name);
        }

        public string Name { get; private set; }

        private MessageServer(string name)
        {
            Name = name;
        }
    }
}