using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using SystemDot.Http;
using SystemDot.Serialisation;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class CoreComponents
    {
        public static void Register()
        {
            MessagingEnvironment.RegisterComponent<IWebRequestor>(new WebRequestor());
            MessagingEnvironment.RegisterComponent<ISerialiser>(new PlatformAgnosticSerialiser());
        }
    }
}