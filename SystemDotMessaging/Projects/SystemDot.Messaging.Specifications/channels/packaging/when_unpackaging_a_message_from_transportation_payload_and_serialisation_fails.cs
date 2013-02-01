using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.packaging
{
    [Subject("Message processing")]
    public class when_unpackaging_a_message_from_transportation_payload_and_serialisation_fails 
    {
        static MessagePayloadUnpackager packager;
        static MessagePayload message;
        static Exception exception;

        Establish context = () =>
        {
            message =  new MessagePayload();
            message.SetBody(new byte[0]);
            packager = new MessagePayloadUnpackager(new FailingSerialiser());
        };

        Because of = () => exception = Catch.Exception(() => packager.InputMessage(message));

        It should_not_fail = () => exception.ShouldBeNull();
    }
}