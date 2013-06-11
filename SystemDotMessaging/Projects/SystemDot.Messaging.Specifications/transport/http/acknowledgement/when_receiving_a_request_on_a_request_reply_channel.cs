using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.http.acknowledgement
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_request_on_a_request_reply_channel : WithServerConfigurationSubject
    {
        const string ReceiverAddress = "ReceiverAddress";
        const string SenderServerAddress = "SenderServerAddress";
        const string SenderServerName = "SenderServer";
        const string ReceiverServerName = "ReceiverServerName";

        static MessagePayload messagePayload;

        Establish context = () =>
        {
            WebRequestor.ExpectAddress(SenderServerName, SenderServerAddress);
            
            Configuration.Configure.Messaging()
                .UsingHttpTransport().AsAServer(ReceiverServerName)
                .OpenChannel(ReceiverAddress).ForRequestReplyRecieving()
                .Initialise();

            messagePayload = new MessagePayload().MakeSequencedReceivable(
                1,
                "SenderAddress",
                SenderServerName, 
                SenderServerName,
                ReceiverAddress,
                ReceiverServerName,
                ReceiverServerName,
                PersistenceUseType.PointToPointSend);

            messagePayload.SetFromServerAddress(SenderServerAddress);
        };

        Because of = () => SendMessagesToServer(messagePayload);

        It should_send_an_acknowledgement_to_the_server_address_specified_on_the_incoming_message = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>().IsAcknowledgement();
    }
}