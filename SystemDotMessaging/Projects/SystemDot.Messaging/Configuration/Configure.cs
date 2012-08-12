using SystemDot.Messaging.Configuration.HttpMessaging;

namespace SystemDot.Messaging.Configuration
{
    public class Configure : Configurer
    {
        public static HttpMessagingConfiguration UsingHttpMessaging()
        {
            return new HttpMessagingConfiguration();
        }
    }
}