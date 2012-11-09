using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Serialisation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.acknowledgement
{
    public class when_handling_a_recieved_acknowledgement_with_a_different_persistence_use_type_to_the_persistence_registered : WithSubject<MessageAcknowledgementHandler>
    {
        static MessagePayload acknowledgement;
        static MessagePayload message;
        static IPersistence differingUseTypePersistence;

        Establish context = () =>
        {
            var store = new InMemoryChangeStore(new PlatformAgnosticSerialiser());

            differingUseTypePersistence = new Persistence(
                store,
                new EndpointAddress("Channel", "Server"),
                PersistenceUseType.RequestSend);

            Subject.RegisterPersistence(differingUseTypePersistence);

            var persistence = new Persistence(
                store,
                differingUseTypePersistence.Address,
                PersistenceUseType.SubscriberRequestSend);

            Subject.RegisterPersistence(persistence);

            message = new MessagePayload();
            var id = new MessagePersistenceId(message.Id, persistence.Address, persistence.UseType);

            acknowledgement = new MessagePayload();
            acknowledgement.SetAcknowledgementId(id);

            differingUseTypePersistence.AddMessageAndIncrementSequence(message);
            persistence.AddMessageAndIncrementSequence(message);
        };

        Because of = () => Subject.InputMessage(acknowledgement);

        It should_not_remove_the_corresponding_message_from_the_message_store = () =>
            differingUseTypePersistence.GetMessages().ShouldContain(message);
    }
}