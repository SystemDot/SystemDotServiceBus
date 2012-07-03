using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using Machine.Fakes;

namespace SystemDot.Messaging.Specifications
{
    public class WithMessageInputterSubject<T> : WithSubject<T>
        where T : class, IMessageInputter<MessagePayload>
    {
        
    }
}