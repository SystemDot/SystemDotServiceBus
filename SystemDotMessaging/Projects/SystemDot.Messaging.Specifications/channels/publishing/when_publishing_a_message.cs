using SystemDot.Messaging.Channels;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.publishing
{
    public class when_publishing_a_message
    {
        static Channel channel;
        static string message;

        Establish context = () =>
        {
            message = "message";

            channel = new Channel(new TestDistributor());
            channel.MessagePublished += o => publishedMessage = o;
            channel.Start();
        };

        Because of = () => channel.Publish(message);

        It should_publish_the_message = () => publishedMessage.ShouldBeTheSameAs(message);

        static object publishedMessage;
    }
}  