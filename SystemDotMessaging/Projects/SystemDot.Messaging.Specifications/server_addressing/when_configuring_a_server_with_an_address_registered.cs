using System;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.server_addressing
{
    public class when_configuring_a_server_with_an_address_registered : WithHttpServerConfigurationSubject
    {
        const string ServerName = "ServerName";
        const string ServerAddress = "ServerAddress";

        Establish context = () => ServerAddressConfiguration.AddAddress(ServerName, ServerAddress);

        Because of = () => Configuration.Configure.Messaging()
            .UsingHttpTransport()
            .AsAServer(ServerName)
            .OpenChannel("SenderAddress").ForPointToPointSendingTo("ReceiverAddress")
            .Initialise();

        It should_listen_a_url_based_on_the_machine_name = () =>
            TestHttpServer.Instance
                .Url.ShouldEqual(String.Concat("http://", ServerAddress, "/", ServerName, "/"));
    }
}