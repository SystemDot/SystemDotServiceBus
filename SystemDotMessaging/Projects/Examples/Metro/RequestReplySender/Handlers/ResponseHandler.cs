using Messages;
using RequestReplySender.ViewModels;

namespace RequestReplySender.Handlers
{
    public class ResponseHandler : IMessageConsumer
    {
        readonly MainPageViewModel viewModel;

        public ResponseHandler(MainPageViewModel viewModel)
        {
            this.viewModel = viewModel;
        }

        public void Handle(TestMessage message)
        {
            this.viewModel.Replies.Add(message.Text);
        }
    }
}