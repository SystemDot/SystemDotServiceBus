using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Storage;
using Machine.Specifications;using FluentAssertions;

namespace SystemDot.Messaging.Specifications.exception_handling_for_subscriptions
{
    [Subject(SpecificationGroup.Description)]
    public class when_failing_the_handling_of_an_event_with_continue_on_exception_configured : WithMessageConfigurationSubject
    {
        const string SubscriberChannel = "SubscriberChannel";
        const string PublisherChannel = "PublisherChannel";
        static Exception exception;

        Establish context = () =>
            Configuration.Configure.Messaging()
                .UsingInProcessTransport()
                .OpenChannel(SubscriberChannel)
                .ForSubscribingTo(PublisherChannel)
                .OnException().ContinueProcessingMessages()
                .RegisterHandlers(r => r.RegisterHandler(new FailingMessageHandler<Int64>()))
                .Initialise();

        Because of = () => exception = Catch.Exception(
            () => GetServer().ReceiveMessage(
                new MessagePayload()
                    .SetMessageBody(1)
                    .SetFromChannel(PublisherChannel)
                    .SetToChannel(SubscriberChannel)
                    .SetChannelType(PersistenceUseType.PublisherSend)
                    .Sequenced()));

        It should_not_throw_an_exception = () => exception.Should().BeNull();
    }
}
