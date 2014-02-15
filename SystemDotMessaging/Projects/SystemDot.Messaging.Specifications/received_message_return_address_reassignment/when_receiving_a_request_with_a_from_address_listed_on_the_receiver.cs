using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.received_message_return_address_reassignment
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_request_with_a_from_address_listed_on_the_receiver : WithHttpServerConfigurationSubject
    {
        const string SenderServerName = "SenderServerName";
        const string LocalSenderServerAddress = "LocalSenderServerAddress";
        const string ReceiverServerName = "ReceiverServerName";
        const string ReceiverChannel = "ReceiverChannel";

        static MessagePayload messagePayload;

        Establish context = () =>
        {
            ServerAddressConfiguration.AddAddress(SenderServerName, LocalSenderServerAddress);
            
            Configuration.Configure.Messaging()
                .UsingHttpTransport().AsAServer(ReceiverServerName)
                .OpenChannel(ReceiverChannel).ForRequestReplyReceiving()
                .Initialise();

            messagePayload = new MessagePayload()
                .SetMessageBody(1)
                .SetFromChannel("SenderChannel")
                .SetFromServer(SenderServerName)
                .SetFromServerAddress("SenderServerAddress")
                .SetToChannel(ReceiverChannel)
                .SetToServer(ReceiverServerName)
                .SetChannelType(PersistenceUseType.RequestReceive)
                .Sequenced();
        };

        Because of = () => SendMessageToServer(messagePayload);

        It should_send_the_acknoweldgement_to_the_local_address_listed = () => 
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().Server.Address.Path.ShouldBeEquivalentTo(LocalSenderServerAddress);
    }
}