using SystemDot.Messaging.Configuration.ComponentRegistration;

namespace SystemDot.Messaging.Configuration.HttpMessaging
{
    public class HttpMessagingConfiguration
    {
        public MessageServerConfiguration WithLocalMessageServer()
        {
            Components.Register();
            return new MessageServerConfiguration(IocContainer.Resolve<IMachineIdentifier>().GetMachineName());
        }

        public MessageServerConfiguration WithRemoteMessageServer(string serverName)
        {
            Components.Register();
            return new MessageServerConfiguration(serverName);
        }
    }
}