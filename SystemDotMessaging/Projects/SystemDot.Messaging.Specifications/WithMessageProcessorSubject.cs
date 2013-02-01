using SystemDot.Messaging.Packaging;
using Machine.Fakes;

namespace SystemDot.Messaging.Specifications
{
    public class WithMessageProcessorSubject<T> : WithSubject<T>
        where T : class, IMessageProcessor<MessagePayload, MessagePayload>
    {

    }
}