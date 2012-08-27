using SystemDot.Http;
using SystemDot.Messaging.Messages;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Transport.Http;
using SystemDot.Messaging.Transport.Http.LongPolling;
using SystemDot.Serialisation;
using Machine.Fakes;
using Machine.Specifications;
using SystemDot.Messaging.Messages.Packaging.Headers;

namespace SystemDot.Messaging.Specifications.transport.long_polling
{
    [Subject("Message sending")]
    public class when_sending_a_message : WithSubject<MessageSender>
    {
        static MessagePayload message;
        static TestSerialiser serialiser;

        Establish context = () =>
        {
            message = new MessagePayload();
            message.SetToAddress(new EndpointAddress("Channel", "Server"));
            serialiser = new TestSerialiser();

            Configure<ISerialiser>(serialiser);
            Configure<IWebRequestor>(new TestWebRequestor(The<ISerialiser>(), message.GetToAddress().GetUrl()));
        };

        Because of = () => Subject.InputMessage(message);

        It should_serialise_the_message_and_send_it_as_a_put_request_to_the_message_server = () =>
            serialiser.Graph.ShouldBeTheSameAs(message);
    }
}
