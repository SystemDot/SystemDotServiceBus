using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Sequencing;
using SystemDot.Messaging.Storage;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.sequencing
{
    [Subject("Message processing")]
    public class when_passing_a_message_through_an_outbound_processor : WithMessageProcessorSubject<OutboundSequencer>
    {
        const int Sequence = 1;
        static MessagePayload message;
        static MessagePayload processedMessage;

        Establish context = () =>
        {
            Configure<EndpointAddress>(new EndpointAddress("Channel", "Server"));
            The<IPersistence>().WhenToldTo(p => p.GetNextSequence(The<EndpointAddress>())).Return(Sequence);
            Subject.MessageProcessed += payload => processedMessage = payload;
            message = new MessagePayload();
        };

        Because of = () => Subject.InputMessage(message);

        It should_put_the_new_sequence_on_the_message = () => message.GetSequence().ShouldEqual(Sequence);

        It should_process_the_message_through = () => processedMessage.ShouldBeTheSameAs(message);
    }
}