using SystemDot.Messaging.Test.Messages;
using SystemDot.Messaging.TestSubscriber.ViewModels;

namespace SystemDot.Messaging.TestSubscriber.Handlers
{
    public class ResponseHandler
    {
        readonly MainPageViewModel viewModel;

        public ResponseHandler(MainPageViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public void Handle(TestMessage message)
        {
            viewModel.Replies.Add(message.Text);
        }
    }
}