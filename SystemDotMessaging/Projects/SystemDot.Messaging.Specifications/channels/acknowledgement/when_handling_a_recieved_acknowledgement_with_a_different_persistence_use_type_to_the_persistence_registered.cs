using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Acknowledgement;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.InMemory;
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
                var store = new InMemoryDatatore();

                differingUseTypePersistence = new InMemoryPersistence(
                    store,
                    PersistenceUseType.RequestSend,
                    new EndpointAddress("Channel", "Server"));

                Subject.RegisterPersistence(differingUseTypePersistence);

                var persistence = new InMemoryPersistence(
                    store,
                    PersistenceUseType.Other,
                    differingUseTypePersistence.Address);

                Subject.RegisterPersistence(persistence);
            
                message = new MessagePayload();
                var id = new MessagePersistenceId(message.Id, persistence.Address, persistence.UseType);

                acknowledgement = new MessagePayload();
                acknowledgement.SetAcknowledgementId(id);

                differingUseTypePersistence.AddMessage(message);
                persistence.AddMessage(message);
            };

        Because of = () => Subject.InputMessage(acknowledgement);

        It should_not_remove_the_corresponding_message_from_the_message_store = () =>
            differingUseTypePersistence.GetMessages().ShouldContain(message);
    }
}