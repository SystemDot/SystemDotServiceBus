using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.Transport.InProcess
{
    public interface IInProcessMessageServer : IMessageProcessor<MessagePayload, MessagePayload>
    {
    }
}