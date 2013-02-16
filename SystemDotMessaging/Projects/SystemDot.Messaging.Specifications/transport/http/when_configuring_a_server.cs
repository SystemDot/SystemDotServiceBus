using System;
using SystemDot.Http.Builders;
using SystemDot.Messaging.Specifications.channels;
using SystemDot.Messaging.Transport.Http.Configuration;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http
{
    [Subject(SpecificationGroup.Description)]
    public class when_configuring_a_server : WithMessageConfigurationSubject
    {
        const string ServerInstance = "ServerInstance";
        
        Establish context = () => ConfigureAndRegister<IHttpServerBuilder>(new TestHttpServerBuilder());

        Because of = () => Configuration.Configure.Messaging()
            .UsingHttpTransport()
            .AsAServer(ServerInstance)
            .OpenChannel("SenderAddress").ForPointToPointSendingTo("ReceiverAddress")
            .Initialise();

        It should_listen_on_the_correct_url = () =>
            TestHttpServer.Instance.Url
                .ShouldEqual(String.Concat("http://", Environment.MachineName, "/", ServerInstance, ":8090/"));
    }
}