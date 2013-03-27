using System.Diagnostics.Contracts;

namespace SystemDot.Messaging.Packaging.Headers
{
    public class BodyHeader : IMessageHeader
    {
        public byte[] Body { get; set; }

        public BodyHeader() {}

        public BodyHeader(byte[] body)
        {
            Contract.Requires(body != null);
            this.Body = body;
        }

        public override string ToString()
        {
            return string.Concat("Body: ", Body.ToString());
        }
    }
}