using SystemDot.Messaging.Configuration.ComponentRegistration;

namespace SystemDot.Messaging.Configuration
{
    public class Configure
    {
        public static MessageServerConfiguration WithLocalMessageServer()
        {
            Components.Register();            
            return new MessageServerConfiguration();
        }
    }
}