using System;
using SystemDot.Messaging;
using Messages;

namespace AuthenticationRequestReplyReciever
{
    public class AuthenticationRequestHandler : IMessageHandler
    {
        public void Handle(AuthenticationRequest message)
        {
            Console.WriteLine("Authenticating");

            if (message.Password == "Hello")
            {
                Console.WriteLine("Authenticated");
                Bus.Reply(new AuthenticatedResponse());
            }
            else
            {
                Console.WriteLine("Authention failed");
                Bus.Reply(new AuthenticationFailedResponse());
            }
        }
    }
}