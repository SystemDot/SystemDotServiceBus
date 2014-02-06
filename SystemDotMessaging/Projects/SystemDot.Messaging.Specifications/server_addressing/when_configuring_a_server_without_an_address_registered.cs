using System;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.server_addressing
{
    [Subject(SpecificationGroup.Description)]
    public class when_configuring_a_server_without_an_address_registered : WithHttpServerConfigurationSubject
    {
        const string ServerName = "ServerName";
        
        Because of = () => Messaging.Configuration.Configure.Messaging()
            .UsingHttpTransport()
            .AsAServer(ServerName)
            .OpenChannel("SenderAddress").ForPointToPointSendingTo("ReceiverAddress")
            .Initialise();

        It should_listen_a_url_based_on_the_machine_name = () =>
            TestHttpServer.Instance
                .Url.ShouldEqual(String.Concat("http://", System.Environment.MachineName, "/", ServerName, "/"));
    }
}