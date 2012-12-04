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
    public class when_handling_a_recieved_acknowledgement_with_a_different_persistence_channel_to_the_persistence_registered : WithSubject<MessageAcknowledgementHandler>
    {
        static MessagePayload acknowledgement;
        static MessagePayload message;
        static MessageCache differingChannelMessageCache;

        Establish context = () =>
        {
            var store = new InMemoryChangeStore(new PlatformAgnosticSerialiser());

            differingChannelMessageCache = new MessageCache(
                store,
                new EndpointAddress("GetChannel", "Server"),
                PersistenceUseType.RequestSend);

            Subject.RegisterCache(differingChannelMessageCache);

            var persistence = new MessageCache(
                store,
                new EndpointAddress("Channel1", "Server1"),
                differingChannelMessageCache.UseType);

            Subject.RegisterCache(persistence);

            message = new MessagePayload();
            var id = new MessagePersistenceId(message.Id, persistence.Address, persistence.UseType);

            acknowledgement = new MessagePayload();
            acknowledgement.SetAcknowledgementId(id);

            differingChannelMessageCache.AddMessageAndIncrementSequence(message);
            persistence.AddMessageAndIncrementSequence(message);
        };

        Because of = () => Subject.InputMessage(acknowledgement);

        It should_not_remove_the_corresponding_message_from_the_message_store = () =>
            differingChannelMessageCache.GetMessages().ShouldContain(message);
    }
}