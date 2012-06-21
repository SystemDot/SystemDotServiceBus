using SystemDot.Messaging.Channels.Messages.Distribution;
using Machine.Fakes;

namespace SystemDot.Messaging.Specifications
{
    public class WithDistributionSubscriberSubject<T> : WithSubject<T> 
        where T : class, IDistributionSubscriber
    {
        
    }
}