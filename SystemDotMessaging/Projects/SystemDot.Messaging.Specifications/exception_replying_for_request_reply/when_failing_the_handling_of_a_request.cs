using System;
using System.Linq;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.RequestReply;
using SystemDot.Messaging.RequestReply.ExceptionHandling;
using SystemDot.Messaging.Specifications.publishing;
using SystemDot.Messaging.Storage;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.exception_replying_for_request_reply
{
    [Subject(SpecificationGroup.Description)]
    public class when_failing_the_handling_of_a_request : WithMessageConfigurationSubject
    {
        static Exception exception;

        Establish context = () =>
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel("ReceiverChannel")
                .ForRequestReplyReceiving()
                .WithDurability()
                .RegisterHandlers(r => r.RegisterHandler(new FailingMessageHandler<Int64>()))
                .Initialise();

        Because of = () => exception = Catch.Exception(() => GetServer().ReceiveMessage(
                new MessagePayload().MakeSequencedReceivable(
                    1,
                    "SenderAddress",
                    "ReceiverChannel",
                    PersistenceUseType.RequestSend)));

        It should_not_pass_the_message_through = () =>
            GetServer().SentMessages.ExcludeAcknowledgements()
                .First().DeserialiseTo<ExceptionOccured>().Message.ShouldEqual(exception.Message);
    }
}
