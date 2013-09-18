using System;
using System.Text;

namespace SystemDot.Messaging.UnitOfWork
{
    public class UnitOfWorkException : Exception
    {
        public UnitOfWorkException(string serialisedMessage, Exception exception)
            : base(GetMessage(serialisedMessage), exception)
        {
        }

        private static string GetMessage(string serialisedMessage)
        {
            var message = new StringBuilder("Unit of work failed for message:");
            message.Append(" ");
            message.Append(serialisedMessage);

            return message.ToString();
        }
    }
}