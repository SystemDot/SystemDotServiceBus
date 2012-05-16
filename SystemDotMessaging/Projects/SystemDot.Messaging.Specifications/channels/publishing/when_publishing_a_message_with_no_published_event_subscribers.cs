using System;
using SystemDot.Messaging.Channels;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.publishing
{
    public class when_publishing_a_message_with_no_published_event_subscribers
    {
        static Channel channel;
        static string message;

        Establish context = () =>
        {
            message = "message";

            channel = new Channel(new TestDistributor());
            channel.Start();
        };

        Because of = () => exception = Catch.Exception(() => channel.Publish(message));

        It should_not_fail = () => exception.ShouldBeNull();

        static Exception exception;
    }
}  