using System;
using System.Linq;
using SystemDot.Http;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Specifications.configuration;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.Http.Configuration;
using SystemDot.Messaging.Transport.Http.Remote.Clients;
using SystemDot.Parallelism;
using SystemDot.Serialisation;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.remote.client
{
    [Subject(SpecificationGroup.Description)]
    public class when_a_message_recieved_has_been_processed : WithConfigurationSubject
    {
        const string ReceiverName = "ReceiverName";
        const string SenderName = "SenderName";

        static TestTaskStarter taskStarter; 
        static MessagePayload messagePayload;
        static TestWebRequestor requestor;

        Establish context = () =>
        {
            requestor = new TestWebRequestor(new PlatformAgnosticSerialiser(), new FixedPortAddress(Environment.MachineName));
            ConfigureAndRegister<IWebRequestor>(requestor);

            taskStarter = new TestTaskStarter(2);
            taskStarter.Pause();

            ConfigureAndRegister<ITaskStarter>(taskStarter); 

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsARemoteClientOf(MessageServer.Local())
                .OpenChannel(ReceiverName)
                .ForPointToPointReceiving()
                .Initialise();

            messagePayload =
                 new MessagePayload()
                    .MakeReceiveable(1, SenderName, ReceiverName, PersistenceUseType.PointToPointSend);
        };

        Because of = () =>
        {
            requestor.AddMessages(messagePayload);
            taskStarter.UnPause();
        };

        It should_poll_again = () => requestor
            .RequestsMade
            .DeserialiseToPayloads()
            .Count(p => p.HasHeader<LongPollRequestHeader>()).ShouldEqual(2);
    }
}