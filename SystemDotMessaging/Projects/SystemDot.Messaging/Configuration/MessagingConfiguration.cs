using System;
using System.Collections.Generic;
using SystemDot.Logging;

namespace SystemDot.Messaging.Configuration
{
    public class MessagingConfiguration
    {
        public List<Action> BuildActions { get; private set; }

        public MessagingConfiguration()
        {
            BuildActions = new List<Action>();
        }

        public MessagingConfiguration LoggingWith(ILoggingMechanism toLogWith)
        {
            Logger.LoggingMechanism = toLogWith;
            return this;
        }
    }
}