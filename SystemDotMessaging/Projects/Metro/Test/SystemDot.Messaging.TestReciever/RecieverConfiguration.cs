using SystemDot.Ioc;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Messages.Processing.Handling;

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