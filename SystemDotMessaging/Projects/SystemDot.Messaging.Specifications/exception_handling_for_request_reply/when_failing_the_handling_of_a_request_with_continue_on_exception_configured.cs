using System;
using System.Linq;
using SystemDot.Messaging.Configuration;
using SystemDot.Messaging.Configuration.RequestReply;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.RequestReply.ExceptionHandling;
using SystemDot.Messaging.Specifications.publishing;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.exception_handling_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_failing_the_handling_of_a_request_with_continue_on_exception_configured : WithMessageConfigurationSubject
    {
        const string ReceiverAddress = "ReceiverAddress";
        static Exception exception;

        Establish context = () =>
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverAddress)
                .ForRequestReplyReceiving()
                .OnException().ContinueProcessingMessages()
                .RegisterHandlers(r => r.RegisterHandler(new FailingMessageHandler<Int64>()))
                .Initialise();

        Because of = () => exception = Catch.Exception(
            () => GetServer().ReceiveMessage(
                new MessagePayload()
                    .SetMessageBody(1)
                    .SetFromChannel("SenderAddress")
                    .SetToChannel(ReceiverAddress)
                    .SetChannelType(PersistenceUseType.RequestSend)
                    .Sequenced()));

        It should_not_throw_an_exception = () => exception.ShouldBeNull();

        It should_reply_with_an_exception_occurred_message = () =>
            GetServer().SentMessages.ExcludeAcknowledgements().First().DeserialiseTo<ExceptionOccured>().Message.ShouldNotBeEmpty();
    }

    [Subject(SpecificationGroup.Description)]
    public class when_failing_the_handling_of_a_reply_with_continue_on_exception_configured : WithMessageConfigurationSubject
    {
        const string SenderAddress = "SenderAddress";
        const string ReceiverAddress = "ReceiverAddress";
        static Exception exception;

        Establish context = () =>
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SenderAddress)
                .ForRequestReplySendingTo(ReceiverAddress)
                .OnException().ContinueProcessingMessages()
                .RegisterHandlers(r => r.RegisterHandler(new FailingMessageHandler<Int64>()))
                .Initialise();

        Because of = () => exception = Catch.Exception(
            () => GetServer().ReceiveMessage(
                new MessagePayload()
                    .SetMessageBody(1)
                    .SetFromChannel(ReceiverAddress)
                    .SetToChannel(SenderAddress)
                    .SetChannelType(PersistenceUseType.ReplySend)
                    .Sequenced()));

        It should_not_throw_an_exception = () => exception.ShouldBeNull();
    }
}
