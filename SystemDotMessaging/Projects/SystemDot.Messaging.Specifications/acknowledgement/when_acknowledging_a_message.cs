using System.Linq;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.acknowledgement
{
    [Subject(SpecificationGroup.Description)]
    public class when_receiveing_a_message : WithMessageConfigurationSubject
    {
        const string ReceiverAddress = "ReceiverAddress";

        static MessagePayload messagePayload;

        Establish context = () =>
        {
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverAddress).ForPointToPointReceiving()
                .Initialise();

            messagePayload = new MessagePayload().MakeSequencedReceivable(
                1,
                "SenderAddress",
                ReceiverAddress,
                PersistenceUseType.PointToPointSend);
        };

        Because of = () => GetServer().ReceiveMessage(messagePayload);

        It should_send_an_acknowledgement_for_the_message_for_the_correct_message_id = () =>
            GetServer().SentMessages.First().GetAcknowledgementId().ShouldEqual(messagePayload.GetSourcePersistenceId());

        It should_send_an_acknowledgement_for_the_message_to_the_message_from_address = () =>
            GetServer().SentMessages.First().GetToAddress().ShouldEqual(messagePayload.GetFromAddress());
    }
}