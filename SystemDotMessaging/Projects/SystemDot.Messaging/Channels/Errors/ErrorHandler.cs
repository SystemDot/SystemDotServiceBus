using System;
using SystemDot.Logging;
using SystemDot.Messaging.Channels.Packaging;

namespace SystemDot.Messaging.Channels.Errors
{
    public class ErrorHandler : IMessageProcessor<MessagePayload, MessagePayload>
    {
        readonly ErrorReciever errorChannelStartPoint;

        public ErrorHandler(ErrorReciever errorChannelStartPoint)
        {
            this.errorChannelStartPoint = errorChannelStartPoint;
        }

        public void InputMessage(MessagePayload toInput)
        {
            try
            {
                MessageProcessed(toInput);
            }
            catch (Exception exception)
            {
                Logger.Error(exception);

                this.errorChannelStartPoint.InputMessage(toInput);
                throw;
            }
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}