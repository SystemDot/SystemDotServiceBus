using System.Runtime.Serialization;
using SystemDot.Http;
using SystemDot.Messaging.Transport.Http.LongPolling;
using SystemDot.Threading;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class LongPollComponents
    {
        public static void Register()
        {
            MessagingEnvironment.RegisterComponent(new LongPollReciever(
                MessagingEnvironment.GetComponent<IWebRequestor>(),
                MessagingEnvironment.GetComponent<IFormatter>()));

            MessagingEnvironment.GetComponent<ThreadedWorkCoordinator>().RegisterWorker(
                MessagingEnvironment.GetComponent<LongPollReciever>());            
        }
    }
}