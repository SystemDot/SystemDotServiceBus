using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class Components 
    {
        public static void Register()
        {
            ThreadingComponents.Register();
            CoreComponents.Register();
            ChannelComponents.Register();
            HttpComponents.Register();
            PublishingComponents.Register();
        }
    }
}