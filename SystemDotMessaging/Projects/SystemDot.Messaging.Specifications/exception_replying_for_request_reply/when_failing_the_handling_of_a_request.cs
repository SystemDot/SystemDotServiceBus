using System;
using System.Linq;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.RequestReply.ExceptionHandling;
using SystemDot.Messaging.Specifications.publishing;
using SystemDot.Messaging.Storage;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.exception_replying_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_failing_the_handling_of_a_request : WithMessageConfigurationSubject
    {
        const string ReceiverAddress = "ReceiverAddress";
        static Exception exception;

        Establish context = () =>
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(ReceiverAddress)
                .ForRequestReplyReceiving()
                .WithDurability()
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

        It should_reply_with_an_exception_occurred_message = () =>
            GetServer().SentMessages.ExcludeAcknowledgements()
                .First().DeserialiseTo<ExceptionOccured>().Message.ShouldBeEquivalentTo(exception.Message);
    }
}
