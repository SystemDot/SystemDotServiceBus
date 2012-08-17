using SystemDot.Messaging.Test.Messages;
using SystemDot.Messaging.TestSubscriber.ViewModels;

namespace SystemDot.Messaging.TestSubscriber.Handlers
{
    public class MessageConsumer
    {
        readonly IBus bus;
        readonly MainPageViewModel viewModel;

        public MessageConsumer(IBus bus, MainPageViewModel viewModel)
        {
            this.bus = bus;
            this.viewModel = viewModel;
        }

        public void Handle(TestMessage message)
        {
            viewModel.Replies.Add(message.Text);
        }
    }
}