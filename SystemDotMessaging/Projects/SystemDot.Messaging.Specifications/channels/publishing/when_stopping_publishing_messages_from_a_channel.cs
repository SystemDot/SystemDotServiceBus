using SystemDot.Messaging.Channels;
using Machine.Specifications;

namespace SystemDot.Messaging.Specifications.channels.publishing
{
    public class when_stopping_publishing_messages_from_a_channel
    {
        static Channel channel;
        static TestDistributor distributor;

        Establish context = () =>
        {
            distributor = new TestDistributor();
            channel = new Channel(distributor);
            channel.Start();
        };

        Because of = () => channel.Stop();

        It should_stop_ditributing_the_messages = () => distributor.IsRunning.ShouldBeFalse();

    }
}  