using SystemDot.Messaging.Configuration.ComponentRegistration;

namespace SystemDot.Messaging.Configuration
{
    public class Configure : Configurer
    {
        public static MessageServerConfiguration WithLocalMessageServer()
        {
            Components.Register();            
            return new MessageServerConfiguration(IocContainer.Resolve<IMachineIdentifier>().GetMachineName());
        }

        public static MessageServerConfiguration WithRemoteMessageServer(string serverName)
        {
            Components.Register();
            return new MessageServerConfiguration(serverName);
        }
    }
}