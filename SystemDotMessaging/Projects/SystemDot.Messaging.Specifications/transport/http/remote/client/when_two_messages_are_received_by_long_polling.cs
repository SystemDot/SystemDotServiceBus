using System;
using System.Linq;
using SystemDot.Messaging.Handling;
using SystemDot.Messaging.Packaging;
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
        const Int64 Message1 = 1;
        const Int64 Message2 = 2;
        const string ServerName = "ServerName";
        const string ProxyName = "ProxyName";

        static TestTaskStarter taskStarter;
        static MessagePayload messagePayload1;
        static MessagePayload messagePayload2;
        static TestMessageHandler<Int64> handler;

        Establish context = () =>
        {
            string serverPath = ServerName + "." + Environment.MachineName;
        
            WebRequestor.ExpectAddress(ProxyName, Environment.MachineName);

            taskStarter = new TestTaskStarter(1);
            taskStarter.Pause(); 
            ConfigureAndRegister<ITaskStarter>(taskStarter);

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                    .AsAServerUsingAProxy(ServerName, ProxyName)
                .OpenChannel(ReceiverName)
                    .ForPointToPointReceiving()
                .Initialise();

            messagePayload1 = new MessagePayload().MakeSequencedReceivable(
                Message1,
                BuildAddressWithProxy(SenderName, serverPath, ProxyName),
                BuildAddressWithProxy(ReceiverName, serverPath, ProxyName),
                PersistenceUseType.PointToPointSend);

            messagePayload2 = new MessagePayload().MakeSequencedReceivable(
                Message2,
                BuildAddressWithProxy(SenderName, serverPath, ProxyName),
                BuildAddressWithProxy(ReceiverName, serverPath, ProxyName),
                PersistenceUseType.PointToPointSend);

            handler = new TestMessageHandler<Int64>();
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