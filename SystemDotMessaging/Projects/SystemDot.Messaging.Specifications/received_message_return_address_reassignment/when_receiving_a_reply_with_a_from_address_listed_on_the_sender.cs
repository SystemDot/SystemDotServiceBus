using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.received_message_return_address_reassignment
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_reply_with_a_from_address_listed_on_the_sender : WithHttpServerConfigurationSubject
    {
        const string ReceiverChannel = "ReceiverChannel";
        const string ReceiverServerName = "ReceiverServerName";
        const string LocalReceiverServerAddress = "LocalReceiverServerAddress";
        const string SenderServerName = "SenderServerName";
        const string SenderChannel = "SenderChannel";
        
        static MessagePayload messagePayload;

        Establish context = () =>
        {
            ServerAddressConfiguration.AddAddress(ReceiverServerName, LocalReceiverServerAddress);
            
            Configuration.Configure.Messaging()
                .UsingHttpTransport().AsAServer(SenderServerName)
                .OpenChannel(SenderChannel).ForRequestReplySendingTo(ReceiverChannel)
                .Initialise();

            messagePayload = new MessagePayload()
                .SetMessageBody(1)
                .SetFromChannel(ReceiverChannel)
                .SetFromServer(ReceiverServerName)
                .SetFromServerAddress("ReceiverServerAddress")
                .SetToChannel(SenderChannel)
                .SetToServer(SenderServerName)
                .SetChannelType(PersistenceUseType.ReplyReceive)
                .Sequenced();
        };

        Because of = () => SendMessageToServer(messagePayload);

        It should_send_the_acknoweldgement_to_the_local_address_listed = () => 
            WebRequestor.DeserialiseSingleRequest<MessagePayload>()
                .GetToAddress().Server.Address.Path.ShouldBeEquivalentTo(LocalReceiverServerAddress);
    }
}