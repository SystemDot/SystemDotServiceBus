using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Packaging;
using Machine.Fakes;

namespace SystemDot.Messaging.Specifications
{
    public class WithMessageInputterSubject<T> : WithSubject<T>
        where T : class, IMessageInputter<MessagePayload>
    {
        
    }
}