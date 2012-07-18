using SystemDot.Messaging.Configuration.ComponentRegistration;

namespace SystemDot.Messaging.Configuration
{
    public class Configure : Configurer
    {
        public static MessageServerConfiguration WithLocalMessageServer()
        {
            Components.Register();            
            return new MessageServerConfiguration(Resolve<IMachineIdentifier>().GetMachineName());
        }
    }
}