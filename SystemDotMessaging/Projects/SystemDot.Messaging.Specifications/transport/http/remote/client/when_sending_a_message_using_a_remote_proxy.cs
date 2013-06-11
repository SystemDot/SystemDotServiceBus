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
        const string Server = "Server";
        const string Proxy = "Proxy";
        const string ProxyAddress = "ProxyAddress";
        const string ChannelAddress = "TestSender";
        const string RecieverAddress = "TestReceiver";
        
        Establish context = () =>
        {
            ConfigureAndRegister<ITaskStarter>();
            
            ServerAddresses.AddAddress(Proxy, ProxyAddress);
            WebRequestor.ExpectAddress(Proxy, ProxyAddress);

            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                    .AsAServerUsingAProxy(Server, Proxy)
                .OpenChannel(ChannelAddress)
                    .ForPointToPointSendingTo(RecieverAddress)
                .Initialise();
        };

        Because of = () => Bus.Send(1);
        
        It should_send_a_message_with_the_to_address_proxy_set_to_the_specified_remote = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().ServerPath.Proxy.Name.ShouldEqual(Proxy);

        It should_send_a_message_with_the_from_address_proxy_set_to_the_specified_remote = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetFromAddress().ServerPath.Proxy.Name.ShouldEqual(Proxy);
    }
}