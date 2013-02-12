using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Sequencing;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Serialisation;
using SystemDot.Storage.Changes;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.acknowledgement
{
    public class when_handling_a_recieved_acknowledgement_with_a_different_persistence_use_type_to_the_persistence_registered : WithSubject<MessageAcknowledgementHandler>
    {
        static MessagePayload acknowledgement;
        static MessagePayload message;
        static MessageCache differingUseTypeMessageCache;

        Establish context = () =>
        {
            var store = new InMemoryChangeStore(new PlatformAgnosticSerialiser());

            differingUseTypeMessageCache = new MessageCache(
                store,
                TestEndpointAddressBuilder.Build("GetChannel", "Server"),
                PersistenceUseType.RequestSend);

            Subject.RegisterCache(differingUseTypeMessageCache);

            var persistence = new MessageCache(
                store,
                differingUseTypeMessageCache.Address,
                PersistenceUseType.SubscriberRequestSend);

            Subject.RegisterCache(persistence);

            message = new MessagePayload();
            message.SetSequence(1);
            var id = new MessagePersistenceId(message.Id, persistence.Address, persistence.UseType);

            acknowledgement = new MessagePayload();
            acknowledgement.SetAcknowledgementId(id);

            differingUseTypeMessageCache.AddMessageAndIncrementSequence(message);
            persistence.AddMessageAndIncrementSequence(message);
        };

        Because of = () => Subject.InputMessage(acknowledgement);

        It should_not_remove_the_corresponding_message_from_the_message_store = () =>
            differingUseTypeMessageCache.GetMessages().ShouldContain(message);
    }
}