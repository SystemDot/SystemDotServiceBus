using System;
using System.Diagnostics.Contracts;
using SystemDot.Logging;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Storage;

namespace SystemDot.Messaging.Acknowledgement
{
    class AcknowledgementSender : IMessageProcessor<MessagePayload>
    {
        public void SendAcknowledgement(MessagePayload toInput)
        {
            Contract.Requires(toInput != null);

            var acknowledgement = new MessagePayload();
            acknowledgement.SetAcknowledgementId(toInput.GetSourcePersistenceId());
            acknowledgement.SetToAddress(toInput.GetFromAddress());

            Logger.Debug("Sending acknowledgement {0} acknowledging message {1}", acknowledgement.Id, toInput.Id);

            MessageProcessed(acknowledgement);
        }

        public event Action<MessagePayload> MessageProcessed;
    }
}