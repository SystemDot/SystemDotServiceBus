using SystemDot.Messaging.Channels.Handling;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.UnitOfWork;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.configuration.request_reply.sending
{
    [Subject("Request reply configuration")]
    public class when_recieving_a_reply_message_with_a_unit_of_work_setup : WithMessageConfigurationSubject
    {
        const string ChannelName = "Test";
        const string RecieverAddress = "TestRecieverAddress";

        static MessagePayload payload;
        static TestMessageHandler<int> handler;

        Establish context = () =>
        {
            ConfigureAndRegister<IUnitOfWork>(new TestUnitOfWork());

            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ChannelName)
                .ForRequestReplySendingTo(RecieverAddress)
                .Initialise();

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);

            payload = CreateRecieveablePayload(1, RecieverAddress, ChannelName, PersistenceUseType.ReplySend);
        };

        Because of = () => MessageReciever.RecieveMessage(payload);

        It should_begin_the_unit_of_work = () =>
            Resolve<IUnitOfWork>()
                .As<TestUnitOfWork>()
                .HasBegun().ShouldBeTrue();
    }

    [Subject("Request reply configuration")]
    public class when_sending_a_local_message : WithMessageConfigurationSubject
    {
        static int message;
        static IBus bus;
        static TestMessageHandler<int> handler;

        Establish context = () =>
        {
            bus = Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("Anything").ForRequestReplyRecieving()
                .Initialise();
            
            message = 1;

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);
        };

        Because of = () => bus.SendLocal(message);

        It should_begin_the_unit_of_work = () => handler.HandledMessage.ShouldEqual(message);

    }
}