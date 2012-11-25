using SystemDot.Ioc;
using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Configuration;
//using SystemDot.Messaging.Storage.Esent;

namespace SystemDot.Messaging.TestReciever
{
    public class RecieverConfiguration
    {
        public static void ConfigureMessaging()
        {
            var bus = Configure.Messaging()
              .UsingInProcessTransport()
              //.UsingFilePersistence("Messaging1")
              .OpenChannel("TestReciever")
                    .ForRequestReplyRecieving()
                    .WithDurability()
              .Initialise();

            IocContainerLocator.Locate()
                .Resolve<MessageHandlerRouter>()
                .RegisterHandler(new RequestHandler(bus));
        }
    }
}