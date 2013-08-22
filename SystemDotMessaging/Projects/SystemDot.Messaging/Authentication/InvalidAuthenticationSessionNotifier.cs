using System.Diagnostics.Contracts;
using SystemDot.Messaging.Packaging;
using SystemDot.Messaging.Packaging.Headers;
using SystemDot.Messaging.Transport;

namespace SystemDot.Messaging.Authentication
{
    class InvalidAuthenticationSessionNotifier
    {
        readonly MessageSender messageSender;

        public InvalidAuthenticationSessionNotifier(MessageSender messageSender)
        {
            Contract.Requires(messageSender != null);
            this.messageSender = messageSender;
        }

        public void Notify(MessagePayload forMessage)
        {
            Contract.Requires(forMessage != null);
            messageSender.InputMessage(CreateInvalidAuthenticationSessionNotification(forMessage));
        }

        static MessagePayload CreateInvalidAuthenticationSessionNotification(MessagePayload forMessage)
        {
            var notification = new MessagePayload();

            notification.SetToAddress(forMessage.GetFromAddress());
            notification.SetFromAddress(forMessage.GetToAddress());

            return notification;
        }
    }
}