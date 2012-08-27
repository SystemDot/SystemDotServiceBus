using System;
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
    public class when_sending_a_message_and_the_web_requestor_throws_an_exception : WithSubject<MessageSender>
    {
        static MessagePayload message;
        static Exception exception;
        
        Establish context = () =>
        {
            Configure<IWebRequestor>(new FailingWebRequestor());
            message = new MessagePayload();
            message.SetToAddress(new EndpointAddress("Channel", "Server"));
        };

        Because of = () => exception = Catch.Exception(() => Subject.InputMessage(message));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}
