using SystemDot.Messaging.Messages.Consuming;
using SystemDot.Messaging.Test.Messages;
using SystemDot.Messaging.TestSubscriber.ViewModels;
using Windows.UI.Core;

namespace SystemDot.Messaging.TestSubscriber.Handlers
{
    public class MessageConsumer : IMessageHandler<TestMessage>
    {
        readonly IBus bus;
        readonly CoreDispatcher dispatcher;
        private MainPageViewModel viewModel;

        public MessageConsumer(IBus bus, CoreDispatcher dispatcher, MainPageViewModel viewModel)
        {
            this.bus = bus;
            this.dispatcher = dispatcher;
            this.viewModel = viewModel;
        }

        public void Handle(TestMessage message)
        {
            this.dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => viewModel.Replies.Add(message.Text));
        }
    }
}