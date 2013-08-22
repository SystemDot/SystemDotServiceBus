using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.authentication
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiving_a_message_without_first_authenticating
        : WithHttpServerConfigurationSubject
    {
        const string ReceiverServer = "ReceiverServer";
        const string ReceiverChannel = "ReceiverChannel";

        static MessagePayload payload;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingHttpTransport()
                .AsAServer(ReceiverServer)
                .RequiresAuthentication()
                .AcceptsRequest<TestAuthenticationRequest>()
                .AuthenticatesOnReply<TestAuthenticationResponse>()
                .OpenChannel(ReceiverChannel)
                .ForPointToPointReceiving()
                .Initialise();

            payload = new MessagePayload()
                .SetMessageBody(1)
                .SetFromChannel("SenderChannel")
                .SetFromServer("SenderServer")
                .SetToChannel(ReceiverChannel)
                .SetToServer(ReceiverServer)
                .SetChannelType(PersistenceUseType.PointToPointSend)
                .Sequenced();
        };

        Because of = () => SendMessagesToServer(payload);
        
        It should_send_back_an_authentication_required_notification_to_the_sender = () => 
            WebRequestor.DeserialiseSingleRequest<MessagePayload>().GetToAddress().ShouldEqual(payload.GetFromAddress());

        It should_send_back_an_authentication_required_notification_from_the_receiver = () =>
            WebRequestor.DeserialiseSingleRequest<MessagePayload>().GetFromAddress().ShouldEqual(payload.GetToAddress());
    }
}