using SystemDot.Messaging.Channels.Messages;
using SystemDot.Messaging.MessageTransportation;
using Machine.Fakes;

namespace SystemDot.Messaging.Specifications
{
    public class WithMessageInputterSubject<T> : WithSubject<T>
        where T : class, IMessageInputter<MessagePayload>
    {
        
    }
}