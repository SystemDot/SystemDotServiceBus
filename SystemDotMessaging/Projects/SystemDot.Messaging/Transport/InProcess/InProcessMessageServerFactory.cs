using SystemDot.Messaging.Transport.Http;

namespace SystemDot.Messaging.Transport.InProcess
{
    class InProcessMessageServerFactory : IInProcessMessageServerFactory
    {
        public IInProcessMessageServer Create(params IMessagingServerHandler[] handlers) 
        { 
            return new InProcessMessageServer(handlers);
        }
    }
}