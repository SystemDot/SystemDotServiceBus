using System;
using SystemDot.Messaging.Acknowledgement;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using SystemDot.Messaging.Storage.Changes;
using SystemDot.Serialisation;
using SystemDot.Specifications;
using SystemDot.Storage.Changes;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.acknowledgement
{
    public class when_handling_a_recieved_acknowledgement : WithSubject<MessageAcknowledgementHandler>
    {
        static MessagePayload acknowledgement;
        static MessagePayload message;
        static SendMessageCache messageCache;

        Establish context = () =>
        {
            var store = new InMemoryChangeStore(new PlatformAgnosticSerialiser());

            messageCache = new SendMessageCache(
                new TestSystemTime(DateTime.Now), 
                store,
                TestEndpointAddressBuilder.Build("GetChannel", "Server"), 
                PersistenceUseType.SubscriberRequestSend);

            Subject.RegisterCache(messageCache);
            
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