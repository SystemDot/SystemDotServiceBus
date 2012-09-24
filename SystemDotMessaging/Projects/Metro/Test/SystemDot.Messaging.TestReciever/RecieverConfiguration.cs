using SystemDot.Ioc;
using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Configuration;

namespace SystemDot.Messaging.TestReciever
{
    public class RecieverConfiguration
    {
        public static void ConfigureMessaging()
        {
            var bus = Configure.Messaging()
              .UsingInProcessTransport()
              .OpenChannel("TestReciever").ForRequestReplyRecieving()
              .Initialise();

            IocContainerLocator.Locate()
                .Resolve<MessageHandlerRouter>()
                .RegisterHandler(new RequestHandler(bus));
        }
    }
}