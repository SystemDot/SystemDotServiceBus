using System.Runtime.Serialization;
using SystemDot.Http;
using SystemDot.Messaging.Transport;
using SystemDot.Messaging.Transport.Http.LongPolling;
using SystemDot.Parallelism;

namespace SystemDot.Messaging.Configuration.ComponentRegistration
{
    public static class HttpComponents
    {
        public static void Register()
        {
            var longPollReciever = new LongPollReciever(
                MessagingEnvironment.GetComponent<IWebRequestor>(), 
                MessagingEnvironment.GetComponent<IFormatter>());

            MessagingEnvironment.GetComponent<TaskLooper>().RegisterToLoop(longPollReciever.Poll);

            MessagingEnvironment.RegisterComponent<IMessageReciever>(longPollReciever);

            MessagingEnvironment.RegisterComponent<IMessageSender>(() => new MessageSender(
                MessagingEnvironment.GetComponent<IFormatter>(),
                MessagingEnvironment.GetComponent<IWebRequestor>()));

        }
    }
}