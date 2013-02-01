using System;
using SystemDot.Http;
using SystemDot.Messaging.Addressing;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Transport.Http;
using SystemDot.Messaging.Transport.Http.LongPolling;
using SystemDot.Serialisation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.long_polling
{
    [Subject("Message sending")]
    public class when_sending_a_message_and_the_web_requestor_throws_an_exception : WithSubject<MessageSender>
    {
        static MessagePayload message;
        static Exception exception;
        
        Establish context = () =>
        {
            Configure<IWebRequestor>(new FailingWebRequestor());
            message = new MessagePayload();
            message.SetToAddress(new EndpointAddress("GetChannel", "Server"));
        };

        Because of = () => exception = Catch.Exception(() => Subject.InputMessage(message));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}
