using System;
using SystemDot.Messaging.Messages.Packaging;
using SystemDot.Messaging.Messages.Processing;
using Machine.Specifications;
using SystemDot.Messaging.Messages.Packaging.Headers;

namespace SystemDot.Messaging.Specifications.messages.processing
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