using System.Linq;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Caching;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.caching
{
    public class when_recording_the_original_persistence_identifier_for_a_message_for_the_second_time 
        : WithSubject<PersistenceSourceRecorder>
    {
        static MessagePayload message;

        Establish context = () =>
        {
            message = new MessagePayload();
            message.SetPersistenceId(TestEndpointAddressBuilder.Build("GetChannel", "Server"), PersistenceUseType.SubscriberSend);
        };

        Because of = () =>
        {
            Subject.InputMessage(message);
            Subject.InputMessage(message);
        };

        It should_not_add_the_source_persistence_header_a_second_time = () =>
            message.Headers.OfType<SourcePersistenceHeader>().Count().ShouldEqual(1);
    }
}