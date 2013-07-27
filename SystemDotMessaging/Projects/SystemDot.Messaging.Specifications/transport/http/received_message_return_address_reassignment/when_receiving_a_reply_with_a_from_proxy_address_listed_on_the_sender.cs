using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.received_message_return_address_reassignment
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_reply_with_a_from_proxy_address_listed_on_the_sender : WithServerConfigurationSubject
    {
        const string ReceiverChannel = "ReceiverChannel";
        const string ReceiverProxyName = "ReceiverProxyName";
        const string LocalReceiverProxyAddress = "LocalReceiverProxyAddress";
        const string SenderServerName = "SenderServerName";
        const string SenderChannel = "SenderChannel";
        
        static MessagePayload messagePayload;

        Establish context = () =>
        {
            ServerAddressConfiguration.AddAddress(ReceiverProxyName, LocalReceiverProxyAddress);
            
            Configuration.Configure.Messaging()
                .UsingHttpTransport().AsAServer(SenderServerName)
                .OpenChannel(SenderChannel).ForRequestReplySendingTo(ReceiverChannel)
                .Initialise();

            messagePayload = new MessagePayload()
                .SetMessageBody(1)
                .SetFromChannel(ReceiverChannel)
                .SetFromProxy(ReceiverProxyName)
                .SetFromProxyAddress("ReceiverProxyAddress")
                .SetToChannel(SenderChannel)
                .SetToServer(SenderServerName)
                .SetChannelType(PersistenceUseType.ReplyReceive)
                .Sequenced();
        };

        Because of = () => SendMessagesToServer(messagePayload);

        It should_send_the_acknoweldgement_to_the_local_proxy_address_listed = () => 
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().Route.Proxy.Address.Path.ShouldEqual(LocalReceiverProxyAddress);
    }
}