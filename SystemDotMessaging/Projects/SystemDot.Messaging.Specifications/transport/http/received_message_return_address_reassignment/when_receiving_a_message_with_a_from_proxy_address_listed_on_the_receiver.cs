using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.received_message_return_address_reassignment
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message_with_a_from_proxy_address_listed_on_the_receiver : WithServerConfigurationSubject
    {
        const string SenderProxyName = "SenderProxyName";
        const string LocalSenderProxyAddress = "LocalSenderProxyAddress";
        const string ReceiverServerName = "ReceiverServerName";
        const string ReceiverChannel = "ReceiverChannel";

        static MessagePayload messagePayload;

        Establish context = () =>
        {
            ServerAddressConfiguration.AddAddress(SenderProxyName, LocalSenderProxyAddress);
            
            Configuration.Configure.Messaging()
                .UsingHttpTransport().AsAServer("ReceiverServerName")
                .OpenChannel("ReceiverChannel").ForPointToPointReceiving()
                .Initialise();

            messagePayload = new MessagePayload()
                .SetMessageBody(1)
                .SetFromChannel("SenderChannel")
                .SetFromProxy(SenderProxyName)
                .SetFromProxyAddress("SenderProxyAddress")
                .SetToChannel(ReceiverChannel)
                .SetToServer(ReceiverServerName)
                .SetChannelType(PersistenceUseType.PointToPointSend)
                .Sequenced();
        };

        Because of = () => SendMessagesToServer(messagePayload);

        It should_send_the_acknoweldgement_to_the_local_proxy_address_listed = () => 
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().Route.Proxy.Address.Path.ShouldEqual(LocalSenderProxyAddress);
    }
}