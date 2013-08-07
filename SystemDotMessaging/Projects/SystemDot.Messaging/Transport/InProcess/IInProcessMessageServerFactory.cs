using SystemDot.Messaging.Transport.Http;

namespace SystemDot.Messaging.Transport.InProcess
{
    public interface IInProcessMessageServerFactory
    {
        IInProcessMessageServer Create(params IMessagingServerHandler[] handlers);
    }
}