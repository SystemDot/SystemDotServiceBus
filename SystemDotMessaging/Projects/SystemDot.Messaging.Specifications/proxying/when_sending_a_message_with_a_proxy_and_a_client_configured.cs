using System.Linq;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Transport.Http.Remote.Clients;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.proxying
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message_with_a_proxy_and_a_client_configured : WithHttpConfigurationSubject
    {
        const string Proxy = "Proxy";
        const string ProxyAddress = "ProxyAddress";

        static TestTaskStarter taskStarter;
        static MessagePayload messagePayload;

        Establish context = () =>
        {
            ServerAddressConfiguration.AddAddress(Proxy, ProxyAddress);

            taskStarter = new TestTaskStarter(1);
            ConfigureAndRegister<ITaskStarter>(taskStarter);
            taskStarter.Pause();

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                    .AsAProxyFor("OtherProxy")
                    .AsAServerUsingAProxy("Server")
                .OpenChannel("ReceiverAddress").ForPointToPointReceiving()
                .Initialise();

             messagePayload = new MessagePayload().MakeReceivable(1, "SenderAddress", "ReceiverAddress", PersistenceUseType.PointToPointSend);
        };

        Because of = () => 
        {
            WebRequestor.AddMessages(messagePayload);
            taskStarter.UnPause();
        };

        It should_long_poll = () => 
            WebRequestor.RequestsMade
                .DeserialiseToPayloads()
                .Count(p => p.HasHeader<LongPollRequestHeader>()).ShouldEqual(1);

    }
}