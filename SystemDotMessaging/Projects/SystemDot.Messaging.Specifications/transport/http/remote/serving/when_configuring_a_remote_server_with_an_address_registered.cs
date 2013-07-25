using System;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.remote.serving
{
    [Subject(SpecificationGroup.Description)]
    public class when_configuring_a_remote_server_with_an_address_registered : WithServerConfigurationSubject
    {
        const string RemoteServerName = "RemoteServerName";
        const string RemoteServerAddress = "RemoteServerAddress";
        
        Establish context = () => ServerAddressConfiguration.AddAddress(RemoteServerName, RemoteServerAddress);

        Because of = () => Configuration.Configure.Messaging()
            .UsingHttpTransport()
            .AsAProxy(RemoteServerName)
            .Initialise();

        It should_listen_on_the_correct_url = () =>
            TestHttpServer.Instance
                .Url.ShouldEqual(String.Concat("http://", RemoteServerAddress, "/", RemoteServerName + "/"));
    }
}