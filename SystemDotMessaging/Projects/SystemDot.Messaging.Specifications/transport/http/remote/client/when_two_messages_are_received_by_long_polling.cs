using System;
using System.Linq;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.remote.client
{
    [Subject(SpecificationGroup.Description)]
    public class when_two_messages_are_received_by_long_polling : WithHttpConfigurationSubject
    {
        const string ReceiverName = "ReceiverName";
        const string SenderName = "SenderName";
        const int Message1 = 1;
        const int Message2 = 2;
        const string RemoteClientName = "RemoteClientName";
        const string ProxyName = "ProxyName";

        static TestTaskStarter taskStarter;
        static MessagePayload messagePayload1;
        static MessagePayload messagePayload2;
        static TestMessageHandler<int> handler;

        Establish context = () =>
        {
            WebRequestor.ExpectAddress(ProxyName, Environment.MachineName);

            taskStarter = new TestTaskStarter(1);
            taskStarter.Pause(); 
            ConfigureAndRegister<ITaskStarter>(taskStarter);

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsARemoteClient(RemoteClientName)
                .UsingProxy(ProxyName)
                .OpenChannel(ReceiverName)
                .ForPointToPointReceiving()
                .Initialise();

            messagePayload1 = new MessagePayload().MakeReceiveable(
                Message1,
                SenderName,
                ReceiverName,
                RemoteClientName,
                ProxyName,
                PersistenceUseType.PointToPointSend);

            messagePayload1.SetSequenceOriginSetOn(DateTime.Today);
            messagePayload1.SetFirstSequence(1);
            messagePayload1.SetSequence(1);

            messagePayload2 = new MessagePayload().MakeReceiveable(
                Message2,
                SenderName,
                ReceiverName,
                RemoteClientName,
                ProxyName,
                PersistenceUseType.PointToPointSend);

            messagePayload2.SetSequenceOriginSetOn(DateTime.Today);
            messagePayload2.SetFirstSequence(1);
            messagePayload2.SetSequence(1);

            handler = new TestMessageHandler<int>();
            Resolve<MessageHandlerRouter>().RegisterHandler(handler);
        };

        Because of = () =>
        {
            WebRequestor.AddMessages(messagePayload1, messagePayload2);
            taskStarter.UnPause();
        };

        It should_output_the_first_recieved_message = () => handler.HandledMessages.First().ShouldEqual(Message1);

        It should_output_the_second_recieved_message = () => handler.HandledMessages.Last().ShouldEqual(Message2);
    }
}