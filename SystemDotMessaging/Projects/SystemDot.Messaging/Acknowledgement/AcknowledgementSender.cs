using System;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Acknowledgement
{
    public class AcknowledgementSender : IMessageProcessor<MessagePayload>
    {
        public void SendAcknowledgement(MessagePayload toInput)
        {
            var acknowledgement = new MessagePayload();
            acknowledgement.SetAcknowledgementId(toInput.GetSourcePersistenceId());
            acknowledgement.SetToAddress(toInput.GetFromAddress());

            MessageProcessed(acknowledgement);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}