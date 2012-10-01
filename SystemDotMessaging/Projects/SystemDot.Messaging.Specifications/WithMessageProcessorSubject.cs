using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Packaging;
using Machine.Fakes;

namespace SystemDot.Messaging.Specifications
{
    public class WithMessageProcessorSubject<T> : WithSubject<T>
        where T : class, IMessageProcessor<MessagePayload, MessagePayload>
    {

    }
}