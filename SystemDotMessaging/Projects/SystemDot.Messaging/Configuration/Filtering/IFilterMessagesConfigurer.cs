using SystemDot.Messaging.Filtering;

namespace SystemDot.Messaging.Configuration.Filtering
{
    public interface IFilterMessagesConfigurer
    {
        void SetMessageFilterStrategy(IMessageFilterStrategy strategy);
    }
}