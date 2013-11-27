using System;
using SystemDot.Messaging.ExceptionHandling;

namespace SystemDot.Messaging.RequestReply.ExceptionHandling
{
    class ExceptionReplier : ExceptionHandler
    {
        public ExceptionReplier(bool shouldContinueOnException) : base(shouldContinueOnException)
        {
        }

        protected override void OnException(Exception exception)
        {
            base.OnException(exception);
            Bus.Reply(new ExceptionOccured { Message = exception.Message });
        }
    }
}