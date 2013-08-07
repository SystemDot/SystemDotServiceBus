using SystemDot.Messaging.Transport.Http;
using SystemDot.Messaging.Transport.InProcess;

namespace SystemDot.Messaging.Specifications
{
    class TestInProcessMessageServerFactory : IInProcessMessageServerFactory
    {
        public IInProcessMessageServer Create(params IMessagingServerHandler[] handlers)
        {
            return new TestInProcessMessageServer(handlers);
        }
    }
}