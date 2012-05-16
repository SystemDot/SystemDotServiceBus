using System.Collections.Generic;
using SystemDot.Messaging.Channels;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.publishing
{
    public class when_publishing_two_messages
    {
        static Channel channel;
        static string message1;
        static string message2;
        static List<object> publishedMessages;
        static TestDistributor distributor;
         
        Establish context = () =>
        {
            message1 = "message1";
            message2 = "message2";
            publishedMessages = new List<object>();

            distributor = new TestDistributor();
            channel = new Channel(distributor);
            channel.MessagePublished += o => publishedMessages.Add(o);
            channel.Start();
        };

        Because of = () =>
        {
            channel.Publish(message1);
            channel.Publish(message2);
        };

        It should_publish_the_first_message = () => publishedMessages.ShouldContain(message1);

        It should_publish_the_second_message = () => publishedMessages.ShouldContain(message2);
    }
}  