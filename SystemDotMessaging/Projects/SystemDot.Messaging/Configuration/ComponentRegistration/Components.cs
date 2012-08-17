using System;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public class Components 
    {
        public static Action Registration { get; set; }

        static Components()
        {
            Registration = RegisterComponents;    
        }

        public static void RegisterComponents()
        {
            ThreadingComponents.Register();
            CoreComponents.Register();
            ChannelComponents.Register();
            HttpComponents.Register();
            PublishingComponents.Register();
            RequestReplyComponents.Register();
        }

        public static void Register()
        {
            Registration();
        }

    }
}