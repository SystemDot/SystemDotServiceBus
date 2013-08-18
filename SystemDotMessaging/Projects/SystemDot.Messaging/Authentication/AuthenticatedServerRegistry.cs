using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using SystemDot.Messaging.Addressing;

namespace SystemDot.Messaging.Authentication
{
    class AuthenticatedServerRegistry
    {
        readonly List<string> servers;

        public AuthenticatedServerRegistry()
        {
            servers = new List<string>();
        }

        public bool Contains(MessageServer toCheck)
        {
            Contract.Requires(toCheck != null);
            return servers.Contains(toCheck.Name);
        }

        public void Register(string toRegister)
        {
            Contract.Requires(!String.IsNullOrEmpty(toRegister));
            servers.Add(toRegister);
        }

        public void Register(MessageServer toRegister)
        {
            Contract.Requires(toRegister != null);
            Register(toRegister.Name);
        }
    }
}