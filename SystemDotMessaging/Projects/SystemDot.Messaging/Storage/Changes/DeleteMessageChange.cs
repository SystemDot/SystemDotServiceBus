using System;

namespace SystemDot.Messaging.Storage.Changes
{
    public class DeleteMessageChange : Change
    {
        public Guid Id { get; set; }

        public DeleteMessageChange()
        {
        }

        public DeleteMessageChange(Guid id)
        {
            Id = id;
        }
    }
}