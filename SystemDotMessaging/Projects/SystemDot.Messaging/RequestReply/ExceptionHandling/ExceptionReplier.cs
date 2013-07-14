using System;
using SystemDot.Messaging.Packaging;

namespace SystemDot.Messaging.RequestReply.ExceptionHandling
{
    class ExceptionReplier : MessageProcessor
    {
        public override void InputMessage(MessagePayload toInput)
        {
            try
            {
                OnMessageProcessed(toInput);
            }
            catch (Exception e)
            {
                Bus.Reply(new ExceptionOccured { Message = e.Message });
                throw;
            }
        }
    }
}