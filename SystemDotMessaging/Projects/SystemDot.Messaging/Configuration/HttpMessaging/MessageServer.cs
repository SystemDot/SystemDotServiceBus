namespace SystemDot.Messaging.Configuration.HttpMessaging
{
    public class MessageServer
    {
        public static MessageServer Local()
        {
            return new MessageServer(IocContainer.Resolve<IMachineIdentifier>().GetMachineName()); 
        }

        public static MessageServer Named(string name)
        {
            return new MessageServer(name);
        }

        public string Name { get; private set; }

        private MessageServer(string name)
        {
            Name = name;
        }
    }
}