using System;
using System.IO;
using SystemDot.Messaging.Transport.Http.LongPolling.Servers;
using SystemDot.Serialisation;
using Machine.Fakes;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.transport.long_polling.serving
{
    [Subject("Message serving")]
    public class when_handling_request_that_fails_deserialisation : WithSubject<HttpMessagingServer>
    {
        static Exception exception;
        
        Establish context = () => Configure<ISerialiser>(new FailingSerialiser());

        Because of = () => exception = Catch.Exception(() => Subject.HandleRequest(new MemoryStream(), new MemoryStream()));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}