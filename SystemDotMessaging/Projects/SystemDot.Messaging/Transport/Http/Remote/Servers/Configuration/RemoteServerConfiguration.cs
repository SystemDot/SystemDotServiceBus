using System;
using System.Collections.Generic;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration;

namespace SystemDot.Messaging.Transport.Http.Remote.Servers.Configuration
{
    public class RemoteServerConfiguration : Initialiser
    {
        readonly ServerPath serverPath;

        public RemoteServerConfiguration(List<Action> actions, ServerPath serverPath)
            : base(actions)
        {
            this.serverPath = serverPath;
        }

        protected override void Build()
        {
        }

        protected override ServerPath GetServerPath()
        {
            return this.serverPath;
        }
    }
}