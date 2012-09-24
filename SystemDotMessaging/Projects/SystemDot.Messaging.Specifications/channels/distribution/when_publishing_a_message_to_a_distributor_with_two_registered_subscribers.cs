using SystemDot.Messaging.Channels;
using SystemDot.Messaging.Channels.Distribution;
using SystemDot.Messaging.Channels.Packaging;
using SystemDot.Messaging.Channels.Packaging.Headers;
using SystemDot.Serialisation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.distribution
{
    [Subject("Message distribution")]
    public class when_publishing_a_message_to_a_distributor_with_two_registered_subscribers : WithSubject<Distributor>
    {
        static MessagePayload inputMessage;
        static Pipe<MessagePayload> subscriber1;
        static Pipe<MessagePayload> subscriber2;
        static MessagePayload processedMessage1;
        static MessagePayload processedMessage2;

        Establish context = () =>
        {
            Configure<ISerialiser>(new PlatformAgnosticSerialiser());
            
            subscriber1 = new Pipe<MessagePayload>();
            subscriber1.MessageProcessed += m => processedMessage1 = m;
            Subject.Subscribe(new object(), subscriber1);

            subscriber2 = new Pipe<MessagePayload>();
            subscriber2.MessageProcessed += m => processedMessage2 = m;
            Subject.Subscribe(new object(), subscriber2);

            inputMessage = new MessagePayload();
            inputMessage.SetToAddress(new EndpointAddress("TestAddress", "TestServer"));
        };

        Because of = () => Subject.InputMessage(inputMessage);

        It should_pass_a_copy_of_the_message_to_the_first_subscriber = () =>
            processedMessage1.GetToAddress().ShouldEqual(inputMessage.GetToAddress());

        It should_pass_a_copy_of_the_message_to_the_second_subscriber = () =>
            processedMessage2.GetToAddress().ShouldEqual(inputMessage.GetToAddress());
    }
}