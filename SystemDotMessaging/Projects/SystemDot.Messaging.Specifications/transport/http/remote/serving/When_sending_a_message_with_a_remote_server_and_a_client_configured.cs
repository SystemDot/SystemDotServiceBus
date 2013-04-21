using System;
using System.Linq;
using SystemDot.Http;
using SystemDot.Http.Builders;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.transport.http.remote.client;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.Http.Configuration;
using SystemDot.Messaging.Transport.Http.Remote.Clients;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.remote.serving
{
    [Subject(SpecificationGroup.Description)]
    public class When_sending_a_message_with_a_remote_server_and_a_client_configured : WithMessageConfigurationSubject
    {
        static TestTaskStarter taskStarter;
        static MessagePayload messagePayload;
        static TestWebRequestor requestor;

        Establish context = () =>
        {
            ConfigureAndRegister<IHttpServerBuilder>(new TestHttpServerBuilder());

            requestor = new TestWebRequestor(
                new PlatformAgnosticSerialiser(),
                new FixedPortAddress(Environment.MachineName, "RemoteServerInstance"));

            ConfigureAndRegister<IWebRequestor>(requestor);

            taskStarter = new TestTaskStarter(1);
            taskStarter.Pause();

            ConfigureAndRegister<ITaskStarter>(taskStarter); 

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsARemoteServer("RemoteServerInstance")
                .AsARemoteClient("RemoteClientInstance")
                .UsingProxy(MessageServer.Local("RemoteServerInstance"))
                .OpenChannel("ReceiverAddress").ForPointToPointReceiving()
                .Initialise();

             messagePayload =
                 new MessagePayload()
                    .MakeReceivable(1, "SenderAddress", "ReceiverAddress", PersistenceUseType.PointToPointSend);
        };

        Because of = () => 
        {
            requestor.AddMessages(messagePayload);
            taskStarter.UnPause();
        };

        It should_long_poll = () => requestor
            .RequestsMade
            .DeserialiseToPayloads()
            .Count(p => p.HasHeader<LongPollRequestHeader>()).ShouldEqual(1);

    }
}