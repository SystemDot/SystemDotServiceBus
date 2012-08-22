using SystemDot.Messaging.Test.Messages;
using SystemDot.Messaging.TestSubscriber.ViewModels;

namespace SystemDot.Messaging.TestSubscriber.Handlers
{
    public class ResponseHandler
    {
        readonly IBus bus;
        readonly MainPageViewModel viewModel;

        public ResponseHandler(IBus bus, MainPageViewModel viewModel)
        {
            this.bus = bus;
            this.viewModel = viewModel;
        }

        public void Handle(TestResponse message)
        {
            viewModel.Replies.Add(message.Text);
        }
    }
}