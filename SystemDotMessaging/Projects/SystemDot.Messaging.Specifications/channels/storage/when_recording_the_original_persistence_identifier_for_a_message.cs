using SystemDot.Messaging.Channels.Addressing;
using SystemDot.Messaging.Channels.Caching;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.storage
{
    public class when_recording_the_original_persistence_identifier_for_a_message 
        : WithMessageProcessorSubject<PersistenceSourceRecorder>
    {
        static MessagePayload message;
        static MessagePayload processedMessage;

        Establish context = () =>
        {
            With<PersistenceBehaviour>();

            message = new MessagePayload();
            message.SetPersistenceId(new EndpointAddress("Channel", "Server"), PersistenceUseType.SubscriberSend);
            Subject.MessageProcessed += m => processedMessage = m;
        };

        Because of = () => Subject.InputMessage(message);

        It should_output_the_message = () => message.ShouldEqual(processedMessage);

        It should_set_the_source_persistence_id_the_same_as_the_persistence_id_on_the_message = () =>
            message.GetSourcePersistenceId()
                .ShouldEqual(new MessagePersistenceId(
                    message.Id,
                    message.GetPersistenceId().Address,
                    message.GetPersistenceId().UseType));
    }
}