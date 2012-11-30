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
    public class when_handling_a_recieved_acknowledgement : WithSubject<MessageAcknowledgementHandler>
    {
        static MessagePayload acknowledgement;
        static MessagePayload message;
        static MessageCache messageCache;

        Establish context = () =>
        {
            var store = new InMemoryChangeStore(new PlatformAgnosticSerialiser());
            
            messageCache = new MessageCache(
                store,
                new EndpointAddress("GetChannel", "Server"), 
                PersistenceUseType.SubscriberRequestSend);

            Subject.RegisterPersistence(messageCache);
            
            message = new MessagePayload();
            var id = new MessagePersistenceId(message.Id, messageCache.Address, messageCache.UseType);
            
            acknowledgement = new MessagePayload();
            acknowledgement.SetAcknowledgementId(id);

            messageCache.AddMessageAndIncrementSequence(message);
        };

        Because of = () => Subject.InputMessage(acknowledgement);

        It should_remove_the_corresponding_message_from_the_message_store = () =>
            messageCache.GetMessages().ShouldNotContain(message);
    }
}