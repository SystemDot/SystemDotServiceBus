using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Parallelism;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.remote.client
{
    [Subject(SpecificationGroup.Description)]
    public class when_sending_a_message_using_a_remote_proxy : WithHttpConfigurationSubject
    {
        const string ClientName = "ClientName";
        const string ProxyName = "ProxyName";
        const string ProxyAddress = "ProxyAddress";
        const string ChannelAddress = "TestSender";
        const string RecieverAddress = "TestReceiver";
        
        Establish context = () =>
        {
            ConfigureAndRegister<ITaskStarter>();
            
            ServerAddresses.AddAddress(ProxyName, ProxyAddress);
            WebRequestor.ExpectAddress(ProxyName, ProxyAddress);

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                    .AsARemoteClient(ClientName)
                    .UsingProxy(ProxyName)
                .OpenChannel(ChannelAddress)
                    .ForPointToPointSendingTo(RecieverAddress)
                .Initialise();
        };

        Because of = () => Bus.Send(1);
        
        It should_send_a_message_with_the_to_address_proxy_set_to_the_specified_remote = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Proxy.Name.ShouldEqual(ProxyName);

        It should_send_a_message_with_the_from_address_proxy_set_to_the_specified_remote = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetFromAddress().ServerPath.Proxy.Name.ShouldEqual(ProxyName);
    }
}