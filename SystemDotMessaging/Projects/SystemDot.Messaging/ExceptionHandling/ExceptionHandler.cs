using System;
using SystemDot.Logging;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.ExceptionHandling
{
    class ExceptionHandler : MessageProcessor
    {
        readonly bool shouldContinueOnException;

        public ExceptionHandler(bool shouldContinueOnException)
        {
            this.shouldContinueOnException = shouldContinueOnException;
        }

        public override void InputMessage(MessagePayload toInput)
        {
            try
            {
                OnMessageProcessed(toInput);
            }
            catch (Exception e)
            {
                if (!shouldContinueOnException) throw;
                
                Logger.Error(e);
            }
        }
    }
}